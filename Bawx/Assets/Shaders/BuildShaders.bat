for %%f in (*.fx) do (

  call 2MGFX %%f %%~nf.ogl.mgfxo /Profile:OpenGL"

  call 2MGFX %%f %%~nf.dx11.mgfxo /Profile:DirectX_11"

)
