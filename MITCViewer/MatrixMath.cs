using System;

using Android.Views;
using Android.Content;
using Android.Util;
using Android.Opengl;
using Android.Graphics;

namespace MITCViewer
{
//2维点
    public class myPoint3DF : Android.Graphics.PointF
    {
        public float Z = 0.0f;
        public myPoint3DF()
        {

        }
    }
    public class myPoint4DF : myPoint3DF
    {
        public float W = 0.0f;
        public myPoint4DF()
        {

        }
    }
public class Tuple2fT
{
  public PointF s ;

  //public float[] T = new float[2];

    public Tuple2fT()
  {
      s = new PointF(0.0f, 0.0f);
  }
} ;     

//3维点
public class Tuple3fT
{
  public myPoint3DF s ;
  //public float[] T = new float[3];

  public Tuple3fT()
  {
      s = new myPoint3DF();
  }
};      

//4维点
public class Tuple4fT
{
  public myPoint4DF s ;
  //public float[] T = new float[4];
  public Tuple4fT()
  {
      s = new myPoint4DF();
  }
};


public class matr33 : ICloneable
{
    //column major
    public float M00; public float XX { set { M00 = value; } get { return M00; } } public float SX { set { M00 = value; } get { return M00; } }
    public float M10; public float XY { set { M10 = value; } get { return M10; } }
    public float M20; public float XZ { set { M20 = value; } get { return M20; } }
    public float M01; public float YX { set { M01 = value; } get { return M01; } }
    public float M11; public float YY { set { M11 = value; } get { return M11; } } public float SY { set { M11 = value; } get { return M11; } }
    public float M21; public float YZ { set { M21 = value; } get { return M21; } }
    public float M02; public float ZX { set { M02 = value; } get { return M02; } }
    public float M12; public float ZY { set { M12 = value; } get { return M12; } }
    public float M22; public float ZZ { set { M22 = value; } get { return M22; } } public float SZ { set { M22 = value; } get { return M22; } }
    public matr33()
    {

    }
    public object Clone()
    {
        matr33 mat = new matr33();
        mat.M00 = this.M00;
        mat.M10 = this.M10;
        mat.M20 = this.M20;
        mat.M01 = this.M01;
        mat.M11 = this.M11;
        mat.M21 = this.M21;
        mat.M02 = this.M02;
        mat.M12 = this.M12;
        mat.M22 = this.M22;

        return mat;
    }
} ;
//3x3矩阵 
public class Matrix3fT : ICloneable
{
    public matr33 s = new matr33();
    //public float[] M = new float[9];
    public Matrix3fT()
    {

    }

    public object Clone()
    {
        Matrix3fT matrx = new Matrix3fT();
        matrx.s = (matr33)this.s.Clone();
        return matrx;
    }
    /*
    public void TOM()
    {
        M[0] = s.M00;
        M[1] = s.M10;
        M[2] = s.M20;
        M[3] = s.M01;
        M[4] = s.M11;
        M[5] = s.M21;
        M[6] = s.M02;
        M[7] = s.M12;
        M[8] = s.M22;
    }
    */
} ;


public class matr44 : ICloneable
    {
        public float M00; public float XX { set { M00 = value; } get { return M00; } } public float SX { set { M00 = value; } get { return M00; } }
        public float M10; public float XY { set { M10 = value; } get { return M10; } }
        public float M20; public float XZ { set { M20 = value; } get { return M20; } }
        public float M30; public float XW { set { M30 = value; } get { return M30; } }
        public float M01; public float YX { set { M01 = value; } get { return M01; } }
        public float M11; public float YY { set { M11 = value; } get { return M11; } } public float SY { set { M11 = value; } get { return M11; } }
        public float M21; public float YZ { set { M21 = value; } get { return M21; } }
        public float M31; public float YW { set { M31 = value; } get { return M31; } }
        public float M02; public float ZX { set { M02 = value; } get { return M02; } }
        public float M12; public float ZY { set { M12 = value; } get { return M12; } }
        public float M22; public float ZZ { set { M22 = value; } get { return M22; } } public float SZ { set { M22 = value; } get { return M22; } }
        public float M32; public float ZW { set { M32 = value; } get { return M32; } }
        public float M03; public float TX { set { M03 = value; } get { return M03; } }
        public float M13; public float TY { set { M13 = value; } get { return M13; } }
        public float M23; public float TZ { set { M23 = value; } get { return M23; } }
        public float M33; public float TW { set { M33 = value; } get { return M33; } } public float SW { set { M33 = value; } get { return M33; } }
        public matr44()
        {

        }
        public object Clone()
        {
            matr44 mat = new matr44();
            mat.M00 = this.M00;
            mat.M10 = this.M10;
            mat.M20 = this.M20;
            mat.M30 = this.M30;
            mat.M01 = this.M01;
            mat.M11 = this.M11;
            mat.M21 = this.M21;
            mat.M31 = this.M31;
            mat.M02 = this.M02;
            mat.M12 = this.M12;
            mat.M22 = this.M22;
            mat.M32 = this.M32;
            mat.M03 = this.M03;
            mat.M13 = this.M13;
            mat.M23 = this.M23;
            mat.M33 = this.M33;
            return mat;
        }
    }
//4x4矩阵
public class Matrix4fT : ICloneable
{
  public matr44 s = new matr44();
  //public float[] M = new float[16];
  public Matrix4fT()
    {
        
    }
  public object Clone()
  {
      Matrix4fT matrx = new Matrix4fT();
      matrx.s = (matr44)this.s.Clone();
      return matrx;
  }
  public float[] TOM()
  {
      float[] M = new float[16];
      M[0] = s.M00;
      M[1] = s.M10;
      M[2] = s.M20;
      M[3] = s.M30;
      M[4] = s.M01;
      M[5] = s.M11;
      M[6] = s.M21;
      M[7] = s.M31;
      M[8] = s.M02;
      M[9] = s.M12;
      M[10] = s.M22;
      M[11] = s.M32;
      M[12] = s.M03;
      M[13] = s.M13;
      M[14] = s.M23;
      M[15] = s.M33;

      return M;
  }
} ;    


//定义类型的别名
    /*
#define Point2fT    Tuple2fT   
#define Quat4fT     Tuple4fT   
#define Vector2fT   Tuple2fT   
#define Vector3fT   Tuple3fT   
#define FuncSqrt    sqrtf
    */

    public class CMatrixMath
    {
        public const float Epsilon = 1.0e-5f;
        //2维点相加
        public static void Point2fAdd(ref Tuple2fT NewObj, Tuple2fT t1)
        {

          NewObj.s.X += t1.s.X;
          NewObj.s.Y += t1.s.Y;
        }

        //2维点相减
public  static void Point2fSub(ref Tuple2fT NewObj, Tuple2fT t1)
{

  NewObj.s.X -= t1.s.X;
  NewObj.s.Y -= t1.s.Y;
}

        //3维点矢积
public  static void Vector3fCross(ref Tuple3fT NewObj,  Tuple3fT v1,  Tuple3fT v2)
{
  NewObj.s.X = (v1.s.Y * v2.s.Z) - (v1.s.Z * v2.s.Y);
  NewObj.s.Y = (v1.s.Z * v2.s.X) - (v1.s.X * v2.s.Z);
  NewObj.s.Z = (v1.s.X * v2.s.Y) - (v1.s.Y * v2.s.X);
}

        //3维点点积
public  static float Vector3fDot( Tuple3fT NewObj,  Tuple3fT v1)
{

  return  (NewObj.s.X * v1.s.X) +
    (NewObj.s.Y * v1.s.Y) +
    (NewObj.s.Z * v1.s.Z);
}

        //3维点的长度的平方
public  static float Vector3fLengthSquared( Tuple3fT NewObj)
{

  return  (NewObj.s.X * NewObj.s.X) +
    (NewObj.s.Y * NewObj.s.Y) +
    (NewObj.s.Z * NewObj.s.Z);
}

        //3维点的长度
public  static float Vector3fLength( Tuple3fT NewObj)
{

  return Convert.ToSingle(Math.Sqrt(Vector3fLengthSquared(NewObj))) ;
}

        //设置3x3矩阵为0矩阵
public  static void Matrix3fSetZero(ref Matrix3fT NewObj)
{
  NewObj.s.M00 = NewObj.s.M01 = NewObj.s.M02 = 
    NewObj.s.M10 = NewObj.s.M11 = NewObj.s.M12 = 
    NewObj.s.M20 = NewObj.s.M21 = NewObj.s.M22 = 0.0f;

}

        //设置4x4矩阵为0矩阵
public  static void Matrix4fSetZero(ref Matrix4fT NewObj)
{
    NewObj.s.M00 = NewObj.s.M01 = NewObj.s.M02 =NewObj.s.M03 = 
    NewObj.s.M10 = NewObj.s.M11 = NewObj.s.M12 =NewObj.s.M13 = 
    NewObj.s.M20 = NewObj.s.M21 = NewObj.s.M22 =NewObj.s.M23 = 
    NewObj.s.M30 = NewObj.s.M31 = NewObj.s.M32 =NewObj.s.M33 = 0.0f;
}

        //设置3x3矩阵为单位矩阵
public  static void Matrix3fSetIdentity(ref Matrix3fT NewObj)
{
  Matrix3fSetZero(ref NewObj);

  NewObj.s.M00 = 
    NewObj.s.M11 = 
    NewObj.s.M22 = 1.0f;

}

        //设置4x4矩阵为单位矩阵
public  static void Matrix4fSetIdentity(ref Matrix4fT NewObj)
{
  Matrix4fSetZero(ref NewObj);

  NewObj.s.M00 = 1.0f;
  NewObj.s.M11 = 1.0f;
  NewObj.s.M22 = 1.0f;
  NewObj.s.M33=1.0f;


}

        //从四元数设置旋转矩阵
public  static void Matrix3fSetRotationFromQuat4f(ref Matrix3fT NewObj,  Tuple4fT q1)
{
  float n, s;
  float xs, ys, zs;
  float wx, wy, wz;
  float xx, xy, xz;
  float yy, yz, zz;


  n = (q1.s.X * q1.s.X) + (q1.s.Y * q1.s.Y) + (q1.s.Z * q1.s.Z) + (q1.s.W * q1.s.W);
  s = (n > 0.0f) ? (2.0f / n) : 0.0f;

  xs = q1.s.X * s;  ys = q1.s.Y * s;  zs = q1.s.Z * s;
  wx = q1.s.W * xs; wy = q1.s.W * ys; wz = q1.s.W * zs;
  xx = q1.s.X * xs; xy = q1.s.X * ys; xz = q1.s.X * zs;
  yy = q1.s.Y * ys; yz = q1.s.Y * zs; zz = q1.s.Z * zs;

  NewObj.s.XX = 1.0f - (yy + zz); NewObj.s.YX =         xy - wz;  NewObj.s.ZX =         xz + wy;
  NewObj.s.XY =         xy + wz;  NewObj.s.YY = 1.0f - (xx + zz); NewObj.s.ZY =         yz - wx;
  NewObj.s.XZ =         xz - wy;  NewObj.s.YZ =         yz + wx;  NewObj.s.ZZ = 1.0f - (xx + yy);

}

//3x3矩阵相乘
public  static void Matrix3fMulMatrix3f(ref Matrix3fT NewObj,  Matrix3fT m1)
{
  Matrix3fT Result = new Matrix3fT(); 


  Result.s.M00 = (NewObj.s.M00 * m1.s.M00) + (NewObj.s.M01 * m1.s.M10) + (NewObj.s.M02 * m1.s.M20);
  Result.s.M01 = (NewObj.s.M00 * m1.s.M01) + (NewObj.s.M01 * m1.s.M11) + (NewObj.s.M02 * m1.s.M21);
  Result.s.M02 = (NewObj.s.M00 * m1.s.M02) + (NewObj.s.M01 * m1.s.M12) + (NewObj.s.M02 * m1.s.M22);

  Result.s.M10 = (NewObj.s.M10 * m1.s.M00) + (NewObj.s.M11 * m1.s.M10) + (NewObj.s.M12 * m1.s.M20);
  Result.s.M11 = (NewObj.s.M10 * m1.s.M01) + (NewObj.s.M11 * m1.s.M11) + (NewObj.s.M12 * m1.s.M21);
  Result.s.M12 = (NewObj.s.M10 * m1.s.M02) + (NewObj.s.M11 * m1.s.M12) + (NewObj.s.M12 * m1.s.M22);

  Result.s.M20 = (NewObj.s.M20 * m1.s.M00) + (NewObj.s.M21 * m1.s.M10) + (NewObj.s.M22 * m1.s.M20);
  Result.s.M21 = (NewObj.s.M20 * m1.s.M01) + (NewObj.s.M21 * m1.s.M11) + (NewObj.s.M22 * m1.s.M21);
  Result.s.M22 = (NewObj.s.M20 * m1.s.M02) + (NewObj.s.M21 * m1.s.M12) + (NewObj.s.M22 * m1.s.M22);

  NewObj = Result;


}

//4x4矩阵相乘
public  static void Matrix4fSetRotationScaleFromMatrix4f(ref Matrix4fT NewObj,  Matrix4fT m1)
{

  NewObj.s.XX = m1.s.XX; NewObj.s.YX = m1.s.YX; NewObj.s.ZX = m1.s.ZX;
  NewObj.s.XY = m1.s.XY; NewObj.s.YY = m1.s.YY; NewObj.s.ZY = m1.s.ZY;
  NewObj.s.XZ = m1.s.XZ; NewObj.s.YZ = m1.s.YZ; NewObj.s.ZZ = m1.s.ZZ;


}

//进行矩阵的奇异值分解，旋转矩阵被保存到rot3和rot4中，返回矩阵的缩放因子
public  static float Matrix4fSVD( Matrix4fT NewObj/*, ref Matrix3fT rot3, ref Matrix4fT rot4*/)
{
  float s;


  s = Convert.ToSingle(Math.Sqrt(
    ( (NewObj.s.XX * NewObj.s.XX) + (NewObj.s.XY * NewObj.s.XY) + (NewObj.s.XZ * NewObj.s.XZ) + 
    (NewObj.s.YX * NewObj.s.YX) + (NewObj.s.YY * NewObj.s.YY) + (NewObj.s.YZ * NewObj.s.YZ) +
    (NewObj.s.ZX * NewObj.s.ZX) + (NewObj.s.ZY * NewObj.s.ZY) + (NewObj.s.ZZ * NewObj.s.ZZ) ) / 3.0f ));

    /*
  //if (rot3 != null)   
  {
    rot3.s.XX = NewObj.s.XX; rot3.s.XY = NewObj.s.XY; rot3.s.XZ = NewObj.s.XZ;
    rot3.s.YX = NewObj.s.YX; rot3.s.YY = NewObj.s.YY; rot3.s.YZ = NewObj.s.YZ;
    rot3.s.ZX = NewObj.s.ZX; rot3.s.ZY = NewObj.s.ZY; rot3.s.ZZ = NewObj.s.ZZ;

    n = 1.0f / Math.Sqrt( (NewObj.s.XX * NewObj.s.XX) +
      (NewObj.s.XY * NewObj.s.XY) +
      (NewObj.s.XZ * NewObj.s.XZ) );
    rot3.s.XX *= n;
    rot3.s.XY *= n;
    rot3.s.XZ *= n;

    n = 1.0f / Math.Sqrt( (NewObj.s.YX * NewObj.s.YX) +
      (NewObj.s.YY * NewObj.s.YY) +
      (NewObj.s.YZ * NewObj.s.YZ) );
    rot3.s.YX *= n;
    rot3.s.YY *= n;
    rot3.s.YZ *= n;

    n = 1.0f / Math.Sqrt( (NewObj.s.ZX * NewObj.s.ZX) +
      (NewObj.s.ZY * NewObj.s.ZY) +
      (NewObj.s.ZZ * NewObj.s.ZZ) );
    rot3.s.ZX *= n;
    rot3.s.ZY *= n;
    rot3.s.ZZ *= n;
  }

  //if (rot4)  
  {
    //if (rot4 != NewObj)
    {
      Matrix4fSetRotationScaleFromMatrix4f(ref rot4, NewObj); 
    }


    n = 1.0f / Math.Sqrt( (NewObj.s.XX * NewObj.s.XX) +
      (NewObj.s.XY * NewObj.s.XY) +
      (NewObj.s.XZ * NewObj.s.XZ) );
    rot4.s.XX *= n;
    rot4.s.XY *= n;
    rot4.s.XZ *= n;

    n = 1.0f / Math.Sqrt( (NewObj.s.YX * NewObj.s.YX) +
      (NewObj.s.YY * NewObj.s.YY) +
      (NewObj.s.YZ * NewObj.s.YZ) );
    rot4.s.YX *= n;
    rot4.s.YY *= n;
    rot4.s.YZ *= n;

    n = 1.0f / Math.Sqrt( (NewObj.s.ZX * NewObj.s.ZX) +
      (NewObj.s.ZY * NewObj.s.ZY) +
      (NewObj.s.ZZ * NewObj.s.ZZ) );
    rot4.s.ZX *= n;
    rot4.s.ZY *= n;
    rot4.s.ZZ *= n;
  }
    */
  return s;
}

//从3x3矩阵变为4x4的旋转矩阵
public  static void Matrix4fSetRotationScaleFromMatrix3f(ref Matrix4fT NewObj,  Matrix3fT m1)
{

  NewObj.s.XX = m1.s.XX; NewObj.s.YX = m1.s.YX; NewObj.s.ZX = m1.s.ZX;
  NewObj.s.XY = m1.s.XY; NewObj.s.YY = m1.s.YY; NewObj.s.ZY = m1.s.ZY;
  NewObj.s.XZ = m1.s.XZ; NewObj.s.YZ = m1.s.YZ; NewObj.s.ZZ = m1.s.ZZ;

}

//4x4矩阵的与标量的乘积
public  static void Matrix4fMulRotationScale(ref Matrix4fT NewObj, float scale)
{

  NewObj.s.XX *= scale; NewObj.s.YX *= scale; NewObj.s.ZX *= scale;
  NewObj.s.XY *= scale; NewObj.s.YY *= scale; NewObj.s.ZY *= scale;
  NewObj.s.XZ *= scale; NewObj.s.YZ *= scale; NewObj.s.ZZ *= scale;

}

//设置旋转矩阵
public  static void Matrix4fSetRotationFromMatrix3f(ref Matrix4fT NewObj,  Matrix3fT m1)
{
  float scale;


  scale = Matrix4fSVD(NewObj);

 

  Matrix4fSetRotationScaleFromMatrix3f(ref NewObj, m1);
  Matrix4fMulRotationScale(ref NewObj, scale);

  
}
    }























}