using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bawx
{
    /// <summary>
    ///     An octree implementation where objects must have a bounding box.
    /// </summary>
    internal sealed class BoundingOctree<T> where T : IBounded
    {
        /// <summary>
        ///     The maximum number of elements we can have before we need to subdivide.
        /// </summary>
        private const int MaxItemCount = 48;

        private readonly BoundingOctree<T>[] _children;

        private readonly List<T> _objects;
        private BoundingBox _bounds;

        /// <summary>
        ///     Creates a new octree.
        /// </summary>
        /// <param name="bounds">The octree's bounds.</param>
        public BoundingOctree(BoundingBox bounds)
        {
            _objects = new List<T>(MaxItemCount);
            _children = new BoundingOctree<T>[8];
            _bounds = bounds;
        }

        /// <summary>
        ///     Gets this octree's bounds.
        /// </summary>
        public BoundingBox Bounds
        {
            get { return _bounds; }
        }

        /// <summary>
        ///     Checks to see if this octree has divided.
        /// </summary>
        public bool HasDivided
        {
            get { return _children[0] != null; }
        }

        /// <summary>
        /// Checks to see if this octree's bounds contains or intersects the given objects bounding box.
        /// </summary>
        public bool Contains(IBounded obj)
        {
            return Contains(obj.Bounds);
        }

        /// <summary>
        ///     Checks to see if this octree's bounds contains or intersects another bounding box.
        /// </summary>
        public bool Contains(BoundingBox box)
        {
            var type = _bounds.Contains(box);
            return type == ContainmentType.Contains
                   || type == ContainmentType.Intersects;
        }

        /// <summary>
        ///     Adds an object's bounds to this octree.
        /// </summary>
        public bool Add(T obj)
        {
            // make sure we contain the object
            if (!Contains(obj))
                return false;

            // make sure we can add the object to our children
            if (_objects.Count < MaxItemCount && !HasDivided)
            {
                _objects.Add(obj);
                return true;
            }
            // check if we need to divide
            if (!HasDivided)
                Divide();

            // try to get the child octree that contains the object
            for (var i = 0; i < 8; ++i)
            {
                if (_children[i].Add(obj))
                    return true;
            }

            // honestly, we shouldn't get here
            _objects.Add(obj);
            return true;
        }

        /// <summary>
        ///     Removes the given object's bounds from the octree.
        /// </summary>
        /// <param name="obj">The bounds of the object.</param>
        public bool Remove(T obj)
        {
            // make sure we contain the object
            if (!Contains(obj))
                return false;

            // check if any children contain it first
            if (HasDivided)
            {
                for (var i = 0; i < 8; ++i)
                {
                    if (_children[i].Remove(obj))
                        return true;
                }
            }

            // now we need to check all of our objects
            var index = _objects.IndexOf(obj);
            if (index == -1)
                return false;

            _objects.RemoveAt(index);
            return true;
        }

        /// <summary>
        ///     Clears this octree. Subdivisions, if any, will remain intact but will also be cleared.
        /// </summary>
        public void Clear()
        {
            _objects.Clear();
            if (HasDivided)
            {
                for (var i = 0; i < 8; ++i)
                    _children[i].Clear();
            }
        }

        /// <summary>
        ///     Checks to see if the given objects bounding box collides with this octree.
        /// </summary>
        public bool Collides(IBounded obj)
        {
            return Collides(obj.Bounds);
        }

        /// <summary>
        ///     Checks to see if the given bounding box collides with this octree.
        /// </summary>
        /// <param name="box">The bounds to check.</param>
        /// <returns></returns>
        public bool Collides(BoundingBox box)
        {
            // make sure we at least contain the given bounding box.
            //if ( !Contains( box ) )
            //{
            //    return false;
            //}

            // check children
            if (HasDivided)
            {
                for (var i = 0; i < 8; ++i)
                {
                    if (_children[i].Collides(box))
                    {
                        return true;
                    }
                }
            }

            // check our objects
            for (var i = 0; i < _objects.Count; ++i)
            {
                var type = _objects[i].Bounds.Contains(box);
                if (type == ContainmentType.Contains || type == ContainmentType.Intersects)
                    return true;
            }

            return false;
        }
        /// <summary>
        ///     Checks to see if the given objects bounding box collides with this octree.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <param name="collisions">The list of objects that the given object collides with.</param>
        public bool Collides(IBounded obj, ref List<T> collisions)
        {
            return Collides(obj.Bounds, ref collisions);
        }

        /// <summary>
        ///     Checks to see if the given bounding box collides with this octree.
        /// </summary>
        /// <param name="box">The bounds to check.</param>
        /// <param name="collisions">The list of objects that the given box collides with.</param>
        /// <returns></returns>
        public bool Collides(BoundingBox box, ref List<T> collisions)
        {
            collisions.Clear();

            // make sure we at least contain the given bounding box.
            if (!Contains(box))
            {
                return false;
            }

            // check children
            if (HasDivided)
            {
                for (var i = 0; i < 8; ++i)
                {
                    _children[i].Collides(box, ref collisions);
                }
            }

            // check our blocks
            for (var i = 0; i < _objects.Count; ++i)
            {
                var type = _objects[i].Bounds.Contains(box);
                if (type == ContainmentType.Contains || type == ContainmentType.Intersects)
                {
                    collisions.Add(_objects[i]);
                }
            }

            return collisions.Count > 0;
        }

        /// <summary>
        ///     Gets all of the distances of the intersections a ray makes in this octree.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <returns></returns>
        public List<float> GetIntersectionDistances(Ray ray)
        {
            var dists = new List<float>();
            GetIntersectionDistances(ray, ref dists);
            return dists;
        }

        /// <summary>
        ///     Gets all of the distances of the intersections a ray makes in this octree.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="dists">The list of distances to populate.</param>
        /// <returns></returns>
        public void GetIntersectionDistances(Ray ray, ref List<float> dists)
        {
            dists.Clear();

            // if we've divided, check our children first
            if (HasDivided)
            {
                for (var i = 0; i < 8; ++i)
                {
                    dists.AddRange(_children[i].GetIntersectionDistances(ray));
                }
            }

            // now check our objects
            for (var i = 0; i < _objects.Count; ++i)
            {
                var value = _objects[i].Bounds.Intersects(ray);
                if (value.HasValue)
                {
                    dists.Add(value.Value);
                }
            }
        }

        /// <summary>
        ///     Divides this octree into its eight children.
        /// </summary>
        private void Divide()
        {
            // make sure we haven't divided already
            if (HasDivided)
                throw new InvalidOperationException("This octree has already divided.");

            // get helper variables
            var center = _bounds.GetCenter();
            var qdim = _bounds.GetDimensions()*0.25f;

            // get child centers
            var trb = new Vector3(center.X + qdim.X, center.Y + qdim.Y, center.Z + qdim.Z);
            var trf = new Vector3(center.X + qdim.X, center.Y + qdim.Y, center.Z - qdim.Z);
            var brb = new Vector3(center.X + qdim.X, center.Y - qdim.Y, center.Z + qdim.Z);
            var brf = new Vector3(center.X + qdim.X, center.Y - qdim.Y, center.Z - qdim.Z);
            var tlb = new Vector3(center.X - qdim.X, center.Y + qdim.Y, center.Z + qdim.Z);
            var tlf = new Vector3(center.X - qdim.X, center.Y + qdim.Y, center.Z - qdim.Z);
            var blb = new Vector3(center.X - qdim.X, center.Y - qdim.Y, center.Z + qdim.Z);
            var blf = new Vector3(center.X - qdim.X, center.Y - qdim.Y, center.Z - qdim.Z);

            // create children
            _children[0] = new BoundingOctree<T>(new BoundingBox(tlb - qdim, tlb + qdim)); // top left back
            _children[1] = new BoundingOctree<T>(new BoundingBox(tlf - qdim, tlf + qdim)); // top left front
            _children[2] = new BoundingOctree<T>(new BoundingBox(trb - qdim, trb + qdim)); // top right back
            _children[3] = new BoundingOctree<T>(new BoundingBox(trf - qdim, trf + qdim)); // top right front
            _children[4] = new BoundingOctree<T>(new BoundingBox(blb - qdim, blb + qdim)); // bottom left back
            _children[5] = new BoundingOctree<T>(new BoundingBox(blf - qdim, blf + qdim)); // bottom left front
            _children[6] = new BoundingOctree<T>(new BoundingBox(brb - qdim, brb + qdim)); // bottom right back
            _children[7] = new BoundingOctree<T>(new BoundingBox(brf - qdim, brf + qdim)); // bottom right front

            // go through our items and try to move them into children
            for (var i = 0; i < _objects.Count; ++i)
            {
                for (var j = 0; j < 8; ++j)
                {
                    if (_children[j].Add(_objects[i]))
                    {
                        // move the object from this tree to the child
                        _objects.RemoveAt(i);
                        --i;
                        break;
                    }
                }
            }
        }
    }
}