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
using Android.Graphics;

using Javax.Microedition.Khronos.Egl;
using Javax.Microedition.Khronos.Opengles;

using Android.Opengl;
using Android.Util;

namespace MITCViewer
{
    public class MITCGLSurfaceView : GLSurfaceView
    {
        private ArcBall m_arcball = new ArcBall();


        private System.Boolean m_bTransing = false;   //是否为平移状态

        
        private System.Boolean m_bRotXState = false;   //是否为平移状态
        private System.Boolean m_bRotYState = false;   //是否为平移状态
        private System.Boolean m_bRotZState = false;   //是否为平移状态

        public System.Boolean Transing
        {
            set { m_bTransing = value; }
            get { return m_bTransing; }
        }
        
        public System.Boolean RotXState
        {
            set { m_bRotXState = value; }
            get { return m_bRotXState; }
        }
        public System.Boolean RotYState
        {
            set { m_bRotYState = value; }
            get { return m_bRotYState; }
        }
        public System.Boolean RotZState
        {
            set { m_bRotZState = value; }
            get { return m_bRotZState; }
        }
        

        private PointF m_lastPos = new PointF(0.0f, 0.0f);
        private float m_lastwx = 0.0f;
        private float m_lastwy = 0.0f;
        private float m_lastwz = 0.0f;

        private PointF m_curPos = new PointF(0.0f, 0.0f);
        private float m_curwx = 0.0f;
        private float m_curwy = 0.0f;
        private float m_curwz = 0.0f;


        private bool isDragging = false;

        private Matrix3fT LastRot = new Matrix3fT();
        private Matrix3fT ThisRot = new Matrix3fT();

        private int mOldCounts = 0;
        private float mScaleFactor = 1.0f;
        
        protected float GetLength(PointF pt1, PointF pt2)
        {
            return Convert.ToSingle(Math.Sqrt((pt1.X - pt2.X) * (pt1.X - pt2.X) + (pt1.Y - pt2.Y) * (pt1.Y - pt2.Y)));
        }

        private float mdSizeFactorX = 1.0f;
        private float mdSizeFactorY = 1.0f;
        private int m_intViewWidth = 0;
        private int m_intViewHeight = 0;


           

        private MITCRender mMitcRender = new MITCRender();
        public MITCRender gMitcRender {get{return mMitcRender;}}

     public override void SurfaceChanged(ISurfaceHolder holder, Format format, int w, int h)
        {

            m_intViewWidth = w;
            m_intViewHeight = h;

            if (m_intViewWidth > m_intViewHeight)
            {
                mdSizeFactorX = (float)m_intViewWidth / (float)m_intViewHeight;
                mdSizeFactorY = 1.0f;
            }
            else
            {
                mdSizeFactorX = 1.0f;
                mdSizeFactorY = (float)m_intViewHeight / (float)m_intViewWidth;
            }

            m_arcball.setBounds(m_intViewWidth, m_intViewHeight);



            base.SurfaceChanged( holder,  format,  w,  h);




        }
        public MITCGLSurfaceView(Context context) : base(context)
        {
            isDragging = false;
            CMatrixMath.Matrix3fSetIdentity(ref LastRot);
            CMatrixMath.Matrix3fSetIdentity(ref ThisRot);
            

            try
            {
                // Create an OpenGL ES 2.0 context
                //SetEGLContextClientVersion(1);

                // Set the Renderer for drawing on the GLSurfaceView
                SetRenderer(mMitcRender);

                // Render the view only when there is a change in the drawing
                // data

                this.RenderMode = Android.Opengl.Rendermode.WhenDirty;

                // 注意上面语句的顺序，反了可能会出错

            }
            catch (Exception e)
            {
                //e.printStackTrace();

            }

        }
        private static string TAG = "MyLogCat";

        public override bool OnTouchEvent(MotionEvent evt)
        {

            if (gMitcRender.TransState || gMitcRender.RotAState)
            {
                if (evt.Action == MotionEventActions.Down)
                {
                    m_lastPos.Set(evt.GetX(), evt.GetY());
                    mMitcRender.GetWorldPoint(m_lastPos.X, m_lastPos.Y, ref m_lastwx, ref m_lastwy, ref m_lastwz);
                    m_curPos.X = m_lastPos.X;
                    m_curPos.Y = m_lastPos.Y;

                    m_curwx = m_lastwx;
                    m_curwy = m_lastwy;
                    m_curwz = m_lastwz;
                    Transing = true;
                    //gMitcRender.updateMatrix();
                    if (gMitcRender.RotAState)
                    {
                        Tuple2fT MousePt = new Tuple2fT();
                        MousePt.s.X = evt.GetX();
                        MousePt.s.Y = evt.GetY();
                        isDragging = true;

//                         string strWp = String.Format("Down ThisRot Rot:({0},{1},{2})", ThisRot.s.M00, ThisRot.s.M11, ThisRot.s.M22);
//                         Log.Debug(TAG, strWp);
                        LastRot = (Matrix3fT)ThisRot.Clone();
//                         strWp = String.Format("After Down LastRot Rot:({0},{1},{2})", LastRot.s.M00, LastRot.s.M11, LastRot.s.M22);
//                         Log.Debug(TAG, strWp);
                        m_arcball.click(MousePt);
                    }

                }
                else if (evt.Action == MotionEventActions.Up)
                {
                    Transing = false;
                    if (gMitcRender.RotAState)
                    {
                        isDragging = false;
                    }
                }
                else if (evt.Action == MotionEventActions.Move)
                {
                    m_curPos.Set(evt.GetX(), evt.GetY());
                    mMitcRender.GetWorldPoint(m_curPos.X, m_curPos.Y, ref m_curwx, ref m_curwy, ref m_curwz);
                    mMitcRender.buildTransform((m_curwx - m_lastwx), (m_curwy - m_lastwy), m_curwz - m_lastwz);
                    m_lastPos.X = m_curPos.X;
                    m_lastPos.Y = m_curPos.Y;
                    m_lastwx = m_curwx;
                    m_lastwy = m_curwy;
                    m_lastwz = m_curwz;

                    string strWp = String.Format("1.Move This Point:({0},{1},{2})", m_curwx, m_curwy, m_curwz);
                    Log.Debug(TAG, strWp);

                    if (gMitcRender.RotAState && isDragging)
                    {
                        Tuple4fT ThisQuat = new Tuple4fT();
                        Tuple2fT MousePt = new Tuple2fT();
                        MousePt.s.X = evt.GetX();
                        MousePt.s.Y = evt.GetY();
                        m_arcball.drag(MousePt, ref ThisQuat);
                        //string strWp = String.Format("1.Move This Rot:({0},{1},{2})", ThisRot.s.M00, ThisRot.s.M11, ThisRot.s.M22);
                        //Log.Debug(TAG, strWp);


                        CMatrixMath.Matrix3fSetRotationFromQuat4f(ref ThisRot, ThisQuat);
                        //strWp = String.Format("2.Move This Rot:({0},{1},{2})", ThisRot.s.M00, ThisRot.s.M11, ThisRot.s.M22);
                        //Log.Debug(TAG, strWp);
                        CMatrixMath.Matrix3fMulMatrix3f(ref ThisRot, LastRot);
                        //strWp = String.Format("3.Move This Rot:({0},{1},{2})", ThisRot.s.M00, ThisRot.s.M11, ThisRot.s.M22);
                        //Log.Debug(TAG, strWp);

                        //strWp = String.Format("Move Last Rot:({0},{1},{2})", LastRot.s.M00, LastRot.s.M11, LastRot.s.M22);
                        //Log.Debug(TAG, strWp);
                        Matrix4fT _tempTransform = mMitcRender.Transform;
                        CMatrixMath.Matrix4fSetRotationFromMatrix3f(ref _tempTransform, ThisRot);

                        m_arcball.upstate();
                    }
                    this.RequestRender();
                }
                return true;
            }
            else if (gMitcRender.ScaleState)
            {
                int nCnt = evt.PointerCount;
                
                if ((evt.Action & MotionEventActions.Mask) == MotionEventActions.PointerDown && 2 == nCnt)//<span style="color:#ff0000;">2表示两个手指</span>
                {
                    m_lastPos.Set(evt.GetX(evt.GetPointerId(0)), evt.GetY(evt.GetPointerId(0)));
                    m_curPos.Set(evt.GetX(evt.GetPointerId(nCnt - 1)), evt.GetY(evt.GetPointerId(nCnt - 1)));
                    string strWp = String.Format("2 Down Point:({0}),({1})", m_lastPos.ToString(), m_curPos.ToString());
                    Log.Debug(TAG, strWp);

                    mOldCounts = 2;
                    mScaleFactor = 1.0f;
                    //gMitcRender.updateMatrix();
                }
                else if ((evt.Action & MotionEventActions.Mask) == MotionEventActions.PointerUp && 2 == nCnt)
                {
                    m_lastPos.Set(evt.GetX(evt.GetPointerId(0)), evt.GetY(evt.GetPointerId(0)));
                    m_curPos.Set(evt.GetX(evt.GetPointerId(nCnt - 1)), evt.GetY(evt.GetPointerId(nCnt - 1)));
                    string strWp = String.Format("2 Up Point:({0}),({1})", m_lastPos.ToString(), m_curPos.ToString());
                    Log.Debug(TAG, strWp);
                    mOldCounts = 0;
                    mScaleFactor = 1.0f;


                }
                else if (evt.Action == MotionEventActions.Move && 2 == nCnt)
                {
                    float lastlength = GetLength(m_lastPos, m_curPos);
                    m_lastPos.Set(evt.GetX(evt.GetPointerId(0)), evt.GetY(evt.GetPointerId(0)));
                    m_curPos.Set(evt.GetX(evt.GetPointerId(nCnt - 1)), evt.GetY(evt.GetPointerId(nCnt - 1)));
                    float curlength = GetLength(m_lastPos, m_curPos);
                    mScaleFactor = Math.Abs(lastlength) < 1.0e-6 ? 1.0f : (curlength / lastlength);
                    mMitcRender.buildScale(mScaleFactor);
                    this.RequestRender();
                    string strWp = String.Format("2 Move Point:({0}),({1})", m_lastPos.ToString(), m_curPos.ToString());
                    Log.Debug(TAG, strWp);
                }
            }
            return true;
        }


        
    }
}