using System;
using Android.Views;
using Android.Content;
using Android.Util;
using Android.Opengl;
using Android.Graphics;
namespace MITCViewer
{
class ArcBall
{
  Tuple3fT   StVec = new Tuple3fT();          //保存鼠标点击的坐标
  Tuple3fT EnVec = new Tuple3fT();          //保存鼠标拖动的坐标
  float     AdjustWidth;    //宽度的调整因子
  float     AdjustHeight;   //长度的调整因子
  Matrix4fT Transform = new Matrix4fT();      //计算变换            
  Matrix3fT LastRot = new Matrix3fT();        //上一次的旋转 
  Matrix3fT ThisRot = new Matrix3fT();        //这次的旋转
  float zoomRate;
  float lastZoomRate;

  bool        isDragging;     // 是否拖动
  bool        isRClicked;     // 是否右击鼠标
  bool        isClicked;      // 是否点击鼠标
  bool        isZooming;    //是否正在缩放
  Tuple2fT LastPt = new Tuple2fT();
  Matrix4fT origTransform = new Matrix4fT();
  Tuple2fT MousePt = new Tuple2fT();        // 当前的鼠标位置

  public ArcBall()
  {
      this.StVec.s.X = 0.0f;
      this.StVec.s.Y = 0.0f;
      this.StVec.s.Z = 0.0f;

      this.EnVec.s.X = 0.0f;
      this.EnVec.s.Y = 0.0f;
      this.EnVec.s.Z = 0.0f;


      CMatrixMath.Matrix4fSetIdentity(ref Transform);
      CMatrixMath.Matrix3fSetIdentity(ref LastRot);
      CMatrixMath.Matrix3fSetIdentity(ref ThisRot);

      this.isDragging = false;
      this.isClicked = false;
      this.isRClicked = false;
      this.isZooming = false;
      this.zoomRate = 1;
      this.setBounds(600.0f, 800.0f);
  }
    public ArcBall(float NewWidth, float NewHeight)
    {
        this.StVec.s.X = 0.0f;
        this.StVec.s.Y = 0.0f;
        this.StVec.s.Z = 0.0f;

        this.EnVec.s.X = 0.0f;
        this.EnVec.s.Y = 0.0f;
        this.EnVec.s.Z = 0.0f;


        CMatrixMath.Matrix4fSetIdentity(ref Transform);
        CMatrixMath.Matrix3fSetIdentity(ref LastRot);
        CMatrixMath.Matrix3fSetIdentity(ref ThisRot);

        this.isDragging = false;
        this.isClicked = false;
        this.isRClicked = false;
        this.isZooming = false;
        this.zoomRate = 1;
        this.setBounds(NewWidth, NewHeight);
    }
    public void _mapToSphere(Tuple2fT NewPt, ref Tuple3fT NewVec)
    {
        Tuple2fT TempPt;
        float length;

        //复制到临时变量
        TempPt = NewPt;

        //把长宽调整到[-1 ... 1]区间
        TempPt.s.X = (TempPt.s.X * this.AdjustWidth) - 1.0f;
        TempPt.s.Y = 1.0f - (TempPt.s.Y * this.AdjustHeight);

        //计算长度的平方
        length = (TempPt.s.X * TempPt.s.X) + (TempPt.s.Y * TempPt.s.Y);

        //如果点映射到球的外面
        if (length > 1.0f)
        {
            float norm;

            //缩放到球上
            norm = 1.0f / Convert.ToSingle(Math.Sqrt(length));

            //设置z坐标为0
            NewVec.s.X = TempPt.s.X * norm;
            NewVec.s.Y = TempPt.s.Y * norm;
            NewVec.s.Z = 0.0f;
        }
        //如果在球内
        else
        {
            //利用半径的平方为1,求出z坐标
            NewVec.s.X = TempPt.s.X;
            NewVec.s.Y = TempPt.s.Y;
            NewVec.s.Z = Convert.ToSingle(Math.Sqrt(1.0f - length));
        }
    }

    //设置边界
  public
    void setBounds(float NewWidth, float NewHeight)
  {

    //设置长宽的调整因子
    this.AdjustWidth  = 1.0f / ((NewWidth  - 1.0f) * 0.5f);
    this.AdjustHeight = 1.0f / ((NewHeight - 1.0f) * 0.5f);
  }

  //鼠标点击
  public void click(Tuple2fT NewPt)
  {
     _mapToSphere(NewPt, ref StVec);
  }

  //鼠标拖动计算旋转
  public void drag(Tuple2fT NewPt, ref Tuple4fT NewRot)
  {
      //新的位置
      _mapToSphere(NewPt, ref EnVec);

      //计算旋转
      //if (NewRot)
      {
          Tuple3fT Perp = new Tuple3fT();

          //计算旋转轴
          CMatrixMath.Vector3fCross(ref Perp, StVec, EnVec);

          //如果不为0
          if (CMatrixMath.Vector3fLength(Perp) > CMatrixMath.Epsilon)
          {
              //记录旋转轴
              NewRot.s.X = Perp.s.X;
              NewRot.s.Y = Perp.s.Y;
              NewRot.s.Z = Perp.s.Z;
              //在四元数中,w=cos(a/2)，a为旋转的角度
              NewRot.s.W = CMatrixMath.Vector3fDot(StVec, EnVec);
          }
          //是0，说明没有旋转
          else
          {
              NewRot.s.X =
                NewRot.s.Y =
                NewRot.s.Z =
                NewRot.s.W = 0.0f;
          }
      }
  }

  //更新鼠标状态
  public void    upstate()
  {
      if (!this.isZooming && this.isRClicked)
      {                    // 开始拖动
          this.isZooming = true;                                        // 设置拖动为变量为true        
          this.LastPt = this.MousePt;
          this.lastZoomRate = this.zoomRate;
      }
      else if (this.isZooming)
      {//正在拖动
          if (this.isRClicked)
          {                //拖动        
              CMatrixMath.Point2fSub(ref MousePt, LastPt);
              this.zoomRate = this.lastZoomRate + this.MousePt.s.X * this.AdjustWidth * 2;
          }
          else
          {                                            //停止拖动
              this.isZooming = false;
          }
      }
      else if (!this.isDragging && this.isClicked)
      {                                                // 如果没有拖动
          this.isDragging = true;                                        // 设置拖动为变量为true
          this.LastRot = this.ThisRot;
          this.click(this.MousePt);
      }
      else if (this.isDragging)
      {
          if (this.isClicked)
          {                                            //如果按住拖动
              Tuple4fT ThisQuat = new Tuple4fT();

              this.drag(MousePt, ref ThisQuat);                        // 更新轨迹球的变量
              CMatrixMath.Matrix3fSetRotationFromQuat4f(ref ThisRot, ThisQuat);        // 计算旋转量
              CMatrixMath.Matrix3fMulMatrix3f(ref this.ThisRot, this.LastRot);
              CMatrixMath.Matrix4fSetRotationFromMatrix3f(ref this.Transform, this.ThisRot);
          }
          else                                                        // 如果放开鼠标，设置拖动为false
              this.isDragging = false;
      }
  }
}
}