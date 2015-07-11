using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Javax.Microedition.Khronos.Egl;
using Javax.Microedition.Khronos.Opengles;

using Android.Opengl;
using Java.Nio;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES11;
using OpenTK.Platform;
using OpenTK.Platform.Android;

using Android.Util;

using Java.Lang;
using Android.Graphics;

namespace MITCViewer
{
    public class MITCRender : Java.Lang.Object,GLSurfaceView.IRenderer
    {

        private Matrix4fT m_Transform = new Matrix4fT();

        private int[] viewport = new int[4];
        private float[] mvmatrix = new float[16];
        private float[] projmatrix = new float[16];


        public Matrix4fT Transform
        {
            set { m_Transform = value; }
            get { return m_Transform; }
        }

         private float[] mTriangleArray = {
            -3f,1f,0f,
            -4f,-1f,0f,
            -1f,-1f,0f
    };
         private FloatBuffer mTriangleBuffer;


         private float[] mColorArray ={
            1f,0f,0f,1f,
            0f,1f,0f,1f,
            0f,0f,1f,1f
    };
         private FloatBuffer mColorBuffer;


         private FloatBuffer quateBuffer;
         private float[] mQuateArray = {
            -1f, -1f, 0f,
            1f, -1f, 0f,
            -1f, 1f, 0f,
            1f, 1f, 0f,
    };

         private System.Boolean m_bTransState = false;   //是否为平移状态

         public System.Boolean TransState
         {
             set { m_bTransState = value; }
             get { return m_bTransState; }
         }
         private System.Boolean m_bScaleState = false;   //是否为平移状态

         public System.Boolean ScaleState
         {
             set { m_bScaleState = value; }
             get { return m_bScaleState; }
         }
         private System.Boolean m_bRotAState = false;   //是否为平移状态
         public System.Boolean RotAState
         {
             set { m_bRotAState = value; }
             get { return m_bRotAState; }
         }
         private System.Boolean mbSpecialView = false;
         public System.Boolean SpecialView
         {
             set { mbSpecialView = value; }
             get { return mbSpecialView; }
         }
         public System.Boolean[] ViewIndex
         {
             set { mViewIndex = value; }
             get { return mViewIndex; }
         }

        public MITCRender()
         {
             mTriangleBuffer = FloatbufferUtil(mTriangleArray);
             mColorBuffer = FloatbufferUtil(mColorArray);
             quateBuffer = FloatbufferUtil(mQuateArray);
             CMatrixMath.Matrix4fSetIdentity(ref m_Transform);

          
         }
        private float[] mTransomValue = {1.0f,1.0f,1.0f};
        private float mScaleValue = 1.0f;
        
        public void buildTransform(float dx, float dy, float dz)
        {
            mTransomValue[0] = dx;
            mTransomValue[1] = dy;
            mTransomValue[2] = dz;
        }
        public void buildScale(float dscale)
        {
            mScaleValue = dscale;
        }
        public void updateMatrix()
        {
            GL.GetInteger(All.Viewport, viewport);
            GL.GetFloat(All.ModelviewMatrix, mvmatrix);
            GL.GetFloat(All.ProjectionMatrix, projmatrix);

            string strWp = System.String.Format("\nModelviewMatrix:(\n{0},{1},{2},{3}\n{4},{5},{6},{7}\n{8},{9},{10},{11}\n{12},{13},{14},{15})"
                , mvmatrix[0], mvmatrix[1], mvmatrix[2], mvmatrix[3]
                , mvmatrix[4], mvmatrix[5], mvmatrix[6], mvmatrix[7]
                , mvmatrix[8], mvmatrix[9], mvmatrix[10], mvmatrix[11]
                , mvmatrix[12], mvmatrix[13], mvmatrix[14], mvmatrix[15]);
            Log.Debug("ModelviewMatrix", strWp);

            strWp = System.String.Format("\nProjectionMatrix:(\n{0},{1},{2},{3}\n{4},{5},{6},{7}\n{8},{9},{10},{11}\n{12},{13},{14},{15})"
                , projmatrix[0], projmatrix[1], projmatrix[2], projmatrix[3]
                , projmatrix[4], projmatrix[5], projmatrix[6], projmatrix[7]
                , projmatrix[8], projmatrix[9], projmatrix[10], projmatrix[11]
                , projmatrix[12], projmatrix[13], projmatrix[14], projmatrix[15]);
            Log.Debug("ModelviewMatrix", strWp);

        }
        // This gets called on each frame render
        public void GetWorldPoint(float px, float py, ref float wx, ref float wy, ref float wz)
        {

            float realx = px;
            float realy;  /*  OpenGL y coordinate position  */
            float[] w = new float[4];  /*  returned world x, y, z coords  */

            realy = viewport[3] - py;
            float realz = 0.0f;
            GL.ReadPixels(Convert.ToInt32(realx), Convert.ToInt32(realy), 1, 1, All.DepthComponent32Oes, All.Float, ref realz);

            GLU.GluUnProject(realx, realy, realz, //所求得的物体三维坐标位于近侧裁剪平面上0.0
               mvmatrix, 0, projmatrix, 0, viewport, 0, w, 0);

            wx = w[0];
            wy = w[1];
            wz = w[2];

        }


         public  void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
         {
             // Set the background frame color
             // 启用阴影平滑
             gl.GlShadeModel(GL10.GlSmooth);
             // 黑色背景
             gl.GlClearColor(0.5f, 0.6f, 0.6f, 1.0f);
             // 设置深度缓存
             gl.GlClearDepthf(1.0f);
             // 启用深度测试
             gl.GlEnable(GL10.GlDepthTest);
             // 所作深度测试的类型
             gl.GlDepthFunc(GL10.GlLequal);
             // 告诉系统对透视进行修正
             gl.GlHint(GL10.GlPerspectiveCorrectionHint, GL10.GlNicest);

         }
         public Java.Nio.FloatBuffer createFloatbufferUtil(int length)
         {
             FloatBuffer mBuffer;

             //先初始化buffer,数组的长度*4,因为一个int占4个字节  
             ByteBuffer qbb = ByteBuffer.AllocateDirect(length * 4);
             //数组排列用nativeOrder  
             qbb.Order(ByteOrder.NativeOrder());

             mBuffer = qbb.AsFloatBuffer();
             
             return mBuffer;
         }
         public Java.Nio.IntBuffer createIntbufferUtil(int length)
         {
             IntBuffer mBuffer;

             //先初始化buffer,数组的长度*4,因为一个int占4个字节  
             ByteBuffer qbb = ByteBuffer.AllocateDirect(length * 4);
             //数组排列用nativeOrder  
             qbb.Order(ByteOrder.NativeOrder());

             mBuffer = qbb.AsIntBuffer();

             return mBuffer;
         }
         public Java.Nio.FloatBuffer FloatbufferUtil(float[] arr)
         {
             FloatBuffer mBuffer;
 
             //先初始化buffer,数组的长度*4,因为一个int占4个字节  
             ByteBuffer qbb = ByteBuffer.AllocateDirect(arr.Length * 4);
             //数组排列用nativeOrder  
             qbb.Order(ByteOrder.NativeOrder());
 
             mBuffer = qbb.AsFloatBuffer();
             mBuffer.Put(arr);
             mBuffer.Position(0);
 
             return mBuffer;
         }
         public Java.Nio.IntBuffer IntbufferUtil(int[] arr)
         {
             IntBuffer mBuffer;
 
             //先初始化buffer,数组的长度*4,因为一个int占4个字节  
             ByteBuffer qbb = ByteBuffer.AllocateDirect(arr.Length * 4);
             //数组排列用nativeOrder  
             qbb.Order(ByteOrder.NativeOrder());
 
             mBuffer = qbb.AsIntBuffer();
             mBuffer.Put(arr);
             mBuffer.Position(0);
 
             return mBuffer;
         }  
        protected void DrawAxisArrow(IGL10 gl,float x, float y, float z, float fr = 1.0f, float fg = 0.0f, float fb = 0.0f)
        {
            GL.Disable(All.Texture2D);
            GL.Enable(All.ColorMaterial);

         

            GL.PointSize(18.0f);
            GL.LineWidth(30.0f);

            //GL.EnableClientState(All.ColorArray);
            GL.EnableClientState(All.VertexArray);
            GL.Color4(fr, fg, fb, 1.0f);
            float[] linevertexs_x = { 0.0f, 0.0f, 0.0f, x, y, z };
            GL.VertexPointer(3, All.Float, 0, linevertexs_x);
            GL.DrawArrays(All.Lines, 0, 2);

            
            GL.PushMatrix();


            float arrowlength = 0.2f * Convert.ToSingle(System.Math.Sqrt(x * x + y * y + z * z));
            GL.Translate(x, y, z);

            // 计算长度  
            float length = 0;
            float dir_x = x;
            float dir_y = y;
            float dir_z = z;

            length = Convert.ToSingle(System.Math.Sqrt(dir_x * dir_x + dir_y * dir_y + dir_z * dir_z));
            if (length < 0.0001)
            {
                dir_x = 0.0f; dir_y = 0.0f; dir_z = 1.0f; length = 1.0f;
            }
            dir_x /= length; dir_y /= length; dir_z /= length;
            float up_x, up_y, up_z;
            up_x = 0.0f;
            up_y = 1.0f;
            up_z = 0.0f;
            float side_x, side_y, side_z;
            side_x = up_y * dir_z - up_z * dir_y;
            side_y = up_z * dir_x - up_x * dir_z;
            side_z = up_x * dir_y - up_y * dir_x;
            length = Convert.ToSingle(System.Math.Sqrt(side_x * side_x + side_y * side_y + side_z * side_z));
            if (length < 0.0001)
            {
                side_x = 1.0f; side_y = 0.0f; side_z = 0.0f; length = 1.0f;
            }
            side_x /= length; side_y /= length; side_z /= length;
            up_x = dir_y * side_z - dir_z * side_y;
            up_y = dir_z * side_x - dir_x * side_z;
            up_z = dir_x * side_y - dir_y * side_x;
            // 计算变换矩阵  
            float[] m = { side_x, side_y, side_z, 0.0f,  
        up_x,   up_y,   up_z,   0.0f,  
        dir_x,  dir_y,  dir_z,  0.0f,  
        0.0f,    0.0f,    0.0f,    1.0f };
            GL.MultMatrix(m);


            int idiv = 40;
            float[] vCone = new float[(idiv + 1) * 3 + 3];
            vCone[0] = vCone[1] = 0.0f;
            vCone[2] = arrowlength;

            double ds = System.Math.PI * 2.0 / idiv;
            for (int i = 1; i <= (idiv + 1); i++)
            {
                vCone[3 * i + 0] = Convert.ToSingle(arrowlength * 0.5 * System.Math.Cos((i - 1) * ds));
                vCone[3 * i + 1] = Convert.ToSingle(arrowlength * 0.5 * System.Math.Sin((i - 1) * ds));

                vCone[3 * i + 2] = 0.0f;
            }
            GL.VertexPointer(3, All.Float, 0, vCone);
            GL.DrawArrays(All.TriangleFan, 0, idiv + 2);

            GL.PopMatrix();


            GL.DisableClientState(All.VertexArray);

            GL.Finish();

            GL.Disable(All.ColorMaterial);
            GL.Enable(All.Texture2D);
            //GL.DisableClientState(All.ColorArray);
            /*
            
            gl.GlPointSize(8.0f);
            gl.GlLineWidth(3.0f);

            gl.GlColor4f(fr, fg, fb, 1.0f);
            float[] linevertexs_x = { 0.0f, 0.0f, 0.0f, x, y, z };
            Java.Nio.Buffer linevertexs_x_buf = FloatbufferUtil(linevertexs_x);
            
            gl.GlVertexPointer(3, GL10.GlFloat, 0, linevertexs_x_buf);
            gl.GlDrawArrays(GL10.GlLines, 0, 2);


            gl.GlPushMatrix();


            float arrowlength = 3f;
            gl.GlTranslatef(x, y, z);

            // 计算长度  
            float length = 0;
            float dir_x = x;
            float dir_y = y;
            float dir_z = z;

            length = Convert.ToSingle(System.Math.Sqrt(dir_x * dir_x + dir_y * dir_y + dir_z * dir_z));
            if (length < 0.0001)
            {
                dir_x = 0.0f; dir_y = 0.0f; dir_z = 1.0f; length = 1.0f;
            }
            dir_x /= length; dir_y /= length; dir_z /= length;
            float up_x, up_y, up_z;
            up_x = 0.0f;
            up_y = 1.0f;
            up_z = 0.0f;
            float side_x, side_y, side_z;
            side_x = up_y * dir_z - up_z * dir_y;
            side_y = up_z * dir_x - up_x * dir_z;
            side_z = up_x * dir_y - up_y * dir_x;
            length = Convert.ToSingle(System.Math.Sqrt(side_x * side_x + side_y * side_y + side_z * side_z));
            if (length < 0.0001)
            {
                side_x = 1.0f; side_y = 0.0f; side_z = 0.0f; length = 1.0f;
            }
            side_x /= length; side_y /= length; side_z /= length;
            up_x = dir_y * side_z - dir_z * side_y;
            up_y = dir_z * side_x - dir_x * side_z;
            up_z = dir_x * side_y - dir_y * side_x;
            // 计算变换矩阵  
            float[] m = { side_x, side_y, side_z, 0.0f,  
        up_x,   up_y,   up_z,   0.0f,  
        dir_x,  dir_y,  dir_z,  0.0f,  
        0.0f,    0.0f,    0.0f,    1.0f };

            Java.Nio.FloatBuffer m_buf = FloatbufferUtil(m);

            gl.GlMultMatrixf(m_buf);


            int idiv = 40;
            float[] vCone = new float[(idiv + 1) * 3 + 3];
            vCone[0] = vCone[1] = 0.0f;
            vCone[2] = arrowlength;

            double ds = System.Math.PI * 2.0 / idiv;
            for (int i = 1; i <= (idiv + 1); i++)
            {
                vCone[3 * i + 0] = Convert.ToSingle(arrowlength * 0.5 * System.Math.Cos((i - 1) * ds));
                vCone[3 * i + 1] = Convert.ToSingle(arrowlength * 0.5 * System.Math.Sin((i - 1) * ds));

                vCone[3 * i + 2] = 0.0f;
            }
            FloatBuffer vCone_buf = FloatbufferUtil(vCone);
            gl.GlVertexPointer(3, GL10.GlFloat, 0, vCone_buf);
            gl.GlDrawArrays(GL10.GlTriangleFan, 0, idiv + 2);

            gl.GlPopMatrix();

            gl.GlFinish();
            //gl.GlDisableClientState(GL10.GlVertexArray);
            */

        }
        private float mdSizeFactorX = 1.0f;
        private float mdSizeFactorY = 1.0f;
        private float mdzNear = -1.0f;
        private float mdzFar = 1.0f;
        private System.Boolean mbChangeView = true;

        private System.Boolean[] mViewIndex = { false, false, false, false, false, false };
        public void OnDrawFrame(IGL10 gl)
        {
            if (mbChangeView)
            {
                GL.MatrixMode(All.Projection);
                GL.LoadIdentity();
                GL.Ortho(-mdSizeFactorX, mdSizeFactorX, -mdSizeFactorY, mdSizeFactorY, mdzNear, mdzFar);
                updateMatrix();
                mbChangeView = false;
            }

            
            if (m_bFitAll || m_bViewRest)
            {
                GL.MatrixMode(All.Projection);
                GL.LoadIdentity();
                GL.Ortho(-mdSizeFactorX, mdSizeFactorX, -mdSizeFactorY, mdSizeFactorY, mdzNear, mdzFar);
                //updateMatrix();
                
                //m_bViewRest = false;
            }

            GL.MatrixMode(All.Modelview);
            if (mbSpecialView)
            {
                GL.LoadIdentity();
                CMatrixMath.Matrix4fSetIdentity(ref m_Transform);
                if (mViewIndex[0])      GLU.GluLookAt(gl, 0f, -10f, 0f, 0f, 0.0f, 0f, 0f, 0f, 1f);
                else if (mViewIndex[1]) GLU.GluLookAt(gl, 0f, 10f, 0f, 0f, 0.0f, 0f, 0f, 0f, 1f);
                else if (mViewIndex[2]) GLU.GluLookAt(gl, -10f, 0f, 0f, 0f, 0.0f, 0f, 0f, 0f, 1f);
                else if (mViewIndex[3]) GLU.GluLookAt(gl, 10f, 0f, 0f, 0f, 0.0f, 0f, 0f, 0f, 1f);
                else if (mViewIndex[4])
                {
                    GLU.GluLookAt(gl, 0f, 0f, 10f, 0f, 0.0f, 0f, 0f, 1f, 0f);
                }
                else if (mViewIndex[5]) GLU.GluLookAt(gl, 0f, 0f, -10f, 0f, 0.0f, 0f, 0f, 1f, 0f);

                updateMatrix();
                mbSpecialView = false;
                for (int i = 0; i < 6;i++ )
                {
                    mViewIndex[i] = false;
                }
            }
            //GL.LoadIdentity();
            {
                if (m_bFitAll || m_bViewRest)
                {
                    GL.LoadIdentity();
                    CMatrixMath.Matrix4fSetIdentity(ref m_Transform);
                    updateMatrix();
                    m_bViewRest = false;
                    m_bFitAll = false;
                }
                if (m_bTransState)
                {
                    GL.Translate(mTransomValue[0], mTransomValue[1], mTransomValue[2]);
                }
                if (m_bScaleState)
                {
                    GL.Scale(mScaleValue, mScaleValue, mScaleValue);
                    updateMatrix();
                }
                

                GL.PushMatrix();
                //Transform.TOM();
                GL.MultMatrix(Transform.TOM());
                
                //string str = String.Format("{0},{1},{2}", Transform.M[0], Transform.M[1], Transform.M[2]);
                //Log.Debug(TAG, str);

            }

            gl.GlClear(GL10.GlColorBufferBit | GL10.GlDepthBufferBit);

             DrawAxisArrow(gl, 0f, 0f, 5f, 0f, 0f, 1.0f);
             DrawAxisArrow(gl, 0f, 5f, 0f, 0f, 1f, 0f);
             DrawAxisArrow(gl, 5f, 0f, 0f, 1f, 0f, 0f);


            {
                //glScalef(ArcBall.zoomRate, ArcBall.zoomRate, ArcBall.zoomRate);//2. 缩放
                GL.PopMatrix();
            }
            
            
        }
        protected System.Boolean m_bFitAll = false;
        protected System.Boolean m_bViewRest = false;
        public void OnZoomReset(System.Boolean bReset)
        {
            m_bViewRest = bReset;
            if (m_bViewRest)
            {
                if (viewport[2] > viewport[3])
                {
                    mdSizeFactorY = (float)viewport[2] / viewport[3];
                    mdSizeFactorX = 1.0f;

                    mdSizeFactorX *= 10.0f;
                    mdSizeFactorY *= 10.0f;
                }
                else
                {
                    mdSizeFactorY = (float)viewport[3] / viewport[2];
                    mdSizeFactorX = 1.0f;

                    mdSizeFactorX *= 10.0f;
                    mdSizeFactorY *= 10.0f;
                   
                }
                mdzNear = -20.0f;
                mdzFar = 20.0f;
                //GL.GetFloat(All.ProjectionMatrix, projmatrix);
            }
        }
        public void OnZoomFitAll(System.Boolean bFitAll)
        {
            m_bFitAll = bFitAll;
            if (m_bFitAll)
            {
                if (viewport[2] > viewport[3])
                {
                    mdSizeFactorY = (float)viewport[2] / viewport[3];
                    mdSizeFactorX = 1.0f;
                }
                else
                {
                    mdSizeFactorY = (float)viewport[3] / viewport[2];
                    mdSizeFactorX = 1.0f;
                }
                mdzNear = -1.0f;
                mdzFar = 1.0f;
                //m_bFitAll = false;
                //GL.GetInteger(All.Viewport, viewport);
                //GL.GetFloat(All.ModelviewMatrix, mvmatrix);
                //GL.GetFloat(All.ProjectionMatrix, projmatrix);
            }
//             GL.MatrixMode(All.Projection);
//             //重置投影矩阵
//             GL.LoadIdentity();
// 
//             GL.Ortho(-1.0f, 1.0f, -1.0f, 1.0f, -1.0f, 1.0f);
// 
//             // 选择模型观察矩阵
//             GL.MatrixMode(All.Modelview);
            // 重置模型观察矩阵
            //GL.LoadIdentity();
        }
        public void OnSurfaceChanged(IGL10 gl, int width, int height)
        {
            float ratio = (float)width / height;


            //设置OpenGL场景的大小
            gl.GlViewport(0, 0, width, height);
            //设置投影矩阵
            gl.GlMatrixMode(GL10.GlProjection);
            //重置投影矩阵
            gl.GlLoadIdentity();
            // 设置视口的大小
            viewport[2] = width;
            viewport[3] = height;
            OnZoomReset(true);
            m_bViewRest = false;
            mbChangeView = true;
            //GL.Ortho(-mdSizeFactorX, mdSizeFactorX, -mdSizeFactorY, mdSizeFactorY, mdzNear, mdzFar);
            //updateMatrix();
            
            // Calculate the aspect ratio of the window
            // 选择模型观察矩阵
            gl.GlMatrixMode(GL10.GlModelview);
            // 重置模型观察矩阵
            gl.GlLoadIdentity();

            //             gl.glViewport(0, 0, width, height);
            //             gl.glMatrixMode(GL10.GL_PROJECTION);
            //             gl.glLoadIdentity();
            //             float ratio = (float)width / height;
            //             gl.glFrustumf(-ratio, ratio, -1, 1, 1, 10);



            

            //gl.GlGetIntegerv(GL11.GlViewport, viewport_buf);
            //GLES20.GlGetFloatv(GL11.GlModelviewMatrix, mvmatrix_buf);
            //GLES20.GlGetFloatv(GL11.GlProjectionMatrix, projmatrix_buf);


        }

       
    }
   
}