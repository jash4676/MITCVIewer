using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Javax.Microedition.Khronos.Egl;
using Javax.Microedition.Khronos.Opengles;

using Android.Opengl;


namespace MITCViewer
{
    [Activity(Label = "midas 模型预览", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private MITCGLSurfaceView mGLView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //Boolean bis = hasGLES20();

            // Set our view from the "main" layout resource
            mGLView = new MITCGLSurfaceView(this);
            SetContentView(/*Resource.Layout.Main*/mGLView);
            RegisterForContextMenu(mGLView);
            // Get our button from the layout resource,
            // and attach an event to it
            
        }
        public override void OnCreateContextMenu(IContextMenu menu, View v,
        IContextMenuContextMenuInfo menuInfo)
        {
            MenuInflater.Inflate(Resource.Menu.MenuView, menu);
            base.OnCreateContextMenu(menu, v, menuInfo);
        }
        public override Boolean OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MenuOpt, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override Boolean OnContextItemSelected(IMenuItem item)
        {
            int itemid = item.ItemId;
            string itemtext = item.ToString();
            Toast.MakeText(this, itemtext, ToastLength.Long).Show();
            return base.OnContextItemSelected(item);
        }
        public override Boolean OnOptionsItemSelected(IMenuItem item)
        {
            int itemid = item.ItemId;
            string itemtext = item.ToString();
            
            if (itemid == Resource.Id.menuopt_move)
            {
                mGLView.gMitcRender.TransState = !mGLView.gMitcRender.TransState;
                if (mGLView.gMitcRender.TransState)
                {
                    mGLView.gMitcRender.RotAState = mGLView.gMitcRender.ScaleState = false;
                }
                itemtext += mGLView.gMitcRender.TransState ? "(开)" : "(关)";
                Toast.MakeText(this, itemtext, ToastLength.Long).Show();
                //mGLView.gMitcRender.OnZoomReset(false);
                //mGLView.gMitcRender.OnZoomFitAll(false);
            }
            else if (itemid == Resource.Id.menuopt_rota)
            {
                mGLView.gMitcRender.RotAState = !mGLView.gMitcRender.RotAState;
                if (mGLView.gMitcRender.RotAState)
                {
                    mGLView.gMitcRender.TransState = mGLView.gMitcRender.ScaleState = false;
                }
                itemtext += mGLView.gMitcRender.RotAState ? "(开)" : "(关)";
                Toast.MakeText(this, itemtext, ToastLength.Long).Show();
                //mGLView.gMitcRender.OnZoomReset(false);
                //mGLView.gMitcRender.OnZoomFitAll(false);
            }
            else if (itemid == Resource.Id.menuopt_scale)
            {
                mGLView.gMitcRender.ScaleState = !mGLView.gMitcRender.ScaleState;
                if (mGLView.gMitcRender.ScaleState)
                {
                    mGLView.gMitcRender.TransState = mGLView.gMitcRender.RotAState = false;
                }
                itemtext += mGLView.gMitcRender.ScaleState ? "(开)" : "(关)";
                Toast.MakeText(this, itemtext, ToastLength.Long).Show();
                //mGLView.gMitcRender.OnZoomReset(false);
                //mGLView.gMitcRender.OnZoomFitAll(false);
            }
            else if (itemid == Resource.Id.menuopt_fitall)
            {
                Toast.MakeText(this, itemtext, ToastLength.Long).Show();
                mGLView.gMitcRender.OnZoomFitAll(true);
                mGLView.RequestRender();
                //mGLView.gMitcRender.OnZoomFitAll(false);
            }
            else if (itemid == Resource.Id.menuopt_reset)
            {
                Toast.MakeText(this, itemtext, ToastLength.Long).Show();
                mGLView.gMitcRender.OnZoomReset(true);
                mGLView.RequestRender();
                //mGLView.gMitcRender.OnZoomReset(false);
            }

            else if (itemid == Resource.Id.menuopt_front)
            {
                Toast.MakeText(this, itemtext, ToastLength.Long).Show();
                mGLView.gMitcRender.SpecialView = true;
                mGLView.gMitcRender.ViewIndex[0] = true;
                mGLView.RequestRender();
                //mGLView.gMitcRender.OnZoomReset(false);
            }
            else if (itemid == Resource.Id.menuopt_back)
            {
                Toast.MakeText(this, itemtext, ToastLength.Long).Show();
                mGLView.gMitcRender.SpecialView = true;
                mGLView.gMitcRender.ViewIndex[1] = true;
                mGLView.RequestRender();
                //mGLView.gMitcRender.OnZoomReset(false);
            }
            else if (itemid == Resource.Id.menuopt_left)
            {
                Toast.MakeText(this, itemtext, ToastLength.Long).Show();
                mGLView.gMitcRender.SpecialView = true;
                mGLView.gMitcRender.ViewIndex[2] = true;
                mGLView.RequestRender();
                //mGLView.gMitcRender.OnZoomReset(false);
            }
            else if (itemid == Resource.Id.menuopt_right)
            {
                Toast.MakeText(this, itemtext, ToastLength.Long).Show();
                mGLView.gMitcRender.SpecialView = true;
                mGLView.gMitcRender.ViewIndex[3] = true;
                mGLView.RequestRender();
                //mGLView.gMitcRender.OnZoomReset(false);
            }
            else if (itemid == Resource.Id.menuopt_top)
            {
                Toast.MakeText(this, itemtext, ToastLength.Long).Show();
                mGLView.gMitcRender.SpecialView = true;
                mGLView.gMitcRender.ViewIndex[4] = true;
                mGLView.RequestRender();
                //mGLView.gMitcRender.OnZoomReset(false);
            }
            else if (itemid == Resource.Id.menuopt_bot)
            {
                Toast.MakeText(this, itemtext, ToastLength.Long).Show();
                mGLView.gMitcRender.SpecialView = true;
                mGLView.gMitcRender.ViewIndex[5] = true;
                mGLView.RequestRender();
                //mGLView.gMitcRender.OnZoomReset(false);
            }
            return base.OnOptionsItemSelected(item);
        }
        private Boolean hasGLES20()
        {
            ActivityManager am = (ActivityManager)
                        GetSystemService(Context.ActivityService);
            return am.DeviceConfigurationInfo.ReqGlEsVersion >= 0x20000;
        }
        protected override void OnPause()
        {
            base.OnPause();

            // The following call pauses the rendering thread.
            // If your OpenGL application is memory intensive,
            // you should consider de-allocating objects that
            // consume significant memory here.
            mGLView.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();

            // The following call resumes a paused rendering thread.
            // If you de-allocated graphic objects for onPause()
            // this is a good place to re-allocate them.
            mGLView.OnResume();
        }
    }
}

