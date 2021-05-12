/*******************************************************************************************
*
*   raylib [textures] example - Load textures from raw data
*
*   NOTE: Images are loaded in CPU memory (RAM); textures are loaded in GPU memory (VRAM)
*
*   This example has been created using raylib 1.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Runtime.InteropServices;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.PixelFormat;

namespace Examples
{
    public class textures_raw_data
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [textures] example - texture from raw data");

            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)

            // Load RAW image data (512x512, 32bit RGBA, no file header)
            Image fudesumiRaw = LoadImageRaw("resources/fudesumi.raw", 384, 512, PIXELFORMAT_UNCOMPRESSED_R8G8B8A8, 0);
            Texture2D fudesumi = LoadTextureFromImage(fudesumiRaw);   // Upload CPU (RAM) image to GPU (VRAM)
            UnloadImage(fudesumiRaw);                                 // Unload CPU (RAM) image data

            // Generate a checked texture by code
            int width = 960;
            int height = 480;

            // Store pixel data
            Color[] pixels = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (((x / 32 + y / 32) / 1) % 2 == 0)
                        pixels[y * width + x] = ORANGE;
                    else
                        pixels[y * width + x] = GOLD;
                }
            }

            // Load pixels data into an image structure and create texture
            GCHandle pinnedArray = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            IntPtr pixelPointer = pinnedArray.AddrOfPinnedObject();
            Image checkedIm = new Image
            {
                data = pixelPointer,
                width = width,
                height = height,
                format = PIXELFORMAT_UNCOMPRESSED_R8G8B8A8,
                mipmaps = 1,
            };
            Texture2D checkedTex = LoadTextureFromImage(checkedIm);
            pinnedArray.Free();
            //---------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // TODO: Update your variables here
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawTexture(checkedTex, 0, 0, ColorAlpha(WHITE, 0.5f));
                // DrawTexture(checkedTex, screenWidth / 2 - checkedTex.width / 2, screenHeight / 2 - checkedTex.height / 2, ColorAlpha(WHITE, 0.5f));
                DrawTexture(fudesumi, 430, -30, WHITE);

                DrawText("CHECKED TEXTURE ", 84, 85, 30, BROWN);
                DrawText("GENERATED by CODE", 72, 148, 30, BROWN);
                DrawText("and RAW IMAGE LOADING", 46, 210, 30, BROWN);

                DrawText("(c) Fudesumi sprite by Eiden Marsal", 310, screenHeight - 20, 10, BROWN);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(fudesumi);    // Texture unloading
            UnloadTexture(checkedTex);  // Texture unloading

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
