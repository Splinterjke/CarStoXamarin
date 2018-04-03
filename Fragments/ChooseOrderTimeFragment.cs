using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using System;
using Android.Widget;
using Android.Graphics;
using CarSto.Adapters;
using Android.Util;
using CarSto.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace CarSto.Fragments
{
    public class ChooseOrderTimeFragment : Fragment
    {
        private string choosenLine;
        private string orderId;
        private int lineCount, stoDayWorkLength, workInterval;
        private int ROWS;
        private int GlobalLayoutHeight;
        private int TimeIntervalItemHeight;
        private DateTime workStart, workEnd, now;
        private List<Dictionary<string, object>> schedule;
        CancellationTokenSource tokenSource;
        CancellationToken token;

        private Button acceptTime;
        private LinearLayout gridContainer;
        private TextView selectedDate;
        private ImageView switchDateLeft, switchDateRight;
        private LinearLayout timeIntervalGrid;
        private FrameLayout globalLayout;
        private FrameLayout scheduleContainer;
        private ScrollView scrollViewLayout;

        public ChooseOrderTimeFragment(string orderId, List<Dictionary<string, object>> schedule, int lineCount, DateTime workStart, DateTime workEnd, int workInterval)
        {
            this.orderId = orderId;
            this.schedule = schedule;
            this.lineCount = lineCount;
            this.workStart = workStart;
            this.workEnd = workEnd;
            this.stoDayWorkLength = ((workEnd.Hour * 60 + workEnd.Minute) - (workStart.Hour * 60 + workStart.Minute)) / 60;
            this.workInterval = workInterval;
            this.ROWS = stoDayWorkLength * 2;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_chooseordertime, container, false);
            ToolbarHelper.SetToolbarStyle("Выбор времени", true, this);
            GetElements(view);
            InitData();
            SetEventHandlers();
            return view;
        }

        public override void OnDestroyView()
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
            base.OnDestroyView();
        }

        private void SetEventHandlers()
        {
            acceptTime.Click += async delegate
            {
                var model = new Dictionary<string, object> { { "LineNum", choosenLine }, { "Time", DateTime.Now.AddMinutes(10) } };
                var response = await ClientAPI.PutAsync($"Order/{orderId}/Time", model);
                if (response == null)
                    return;
                this.Activity.SupportFragmentManager.PopBackStack();
            };
        }

        private void InitData()
        {
            acceptTime.Visibility = ViewStates.Gone;
            now = DateTime.Now;
            selectedDate.Text = now.ToShortDateString();
            GlobalLayoutHeight = MinToDp(60 * stoDayWorkLength);
            var globallayoutparam = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, GlobalLayoutHeight);
            globalLayout.LayoutParameters = globallayoutparam;
            DrawTimeIntervalsAndHorizonTalDividers();
            DrawWorkLines();
            //DrawCurrentTimeLine();
            DrawOrders();
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            DrawCurrentTimeLine();
            //await DrawCurrentTimeLine(TimeSpan.FromSeconds(60), token).ConfigureAwait(false);
        }

        private void DrawOrders()
        {
            if (schedule == null || schedule.Count == 0)
                return;
            var length = schedule.Count;
            for (int i = 0; i < length; i++)
            {
                FrameLayout.LayoutParams parms;
                var lineview = gridContainer.FindViewWithTag(schedule[i]["LineNum"].ToString()) as FrameLayout;
                if (lineview == null)
                    continue;
                var workView = new TextView(lineview.Context);
                workView.Tag = $"work{i}";
                workView.SetBackgroundColor(Color.PaleVioletRed);
                var otherworkstarttime = (DateTime)schedule[i]["Time"];
                if (Convert.ToInt32(schedule[i]["Length"]) > 0)
                {
                    parms = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, MinToDp(Convert.ToInt32(schedule[i]["Length"])));
                    workView.Text = $"Заказ-наряд\n№{orderId}\nДлительность: {schedule[i]["Length"]} мин.";
                }
                else
                {
                    parms = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, MinToDp(30));
                    workView.Text = $"Заказ-наряд\n№{orderId}\nДлительность: 0 мин.";
                }
                parms.TopMargin = MinToDp((otherworkstarttime.Hour - workStart.Hour) * 60 + otherworkstarttime.Minute - workStart.Minute);
                workView.LayoutParameters = parms;
                lineview.AddView(workView);
            }
        }

        private bool IsCollising(int[] workLength, List<int[]> workIntervals)
        {
            for (int i = 0; i < workIntervals.Count; i++)
            {
                if ((workIntervals[i][0] <= workLength[0] && workLength[0] <= workIntervals[i][1]) || (workIntervals[i][0] <= workLength[1] && workLength[1] <= workIntervals[i][1]))
                    return true;
            }
            return false;
        }

        private List<int[]> workList;
        private void LineContainer_Click(object sender, EventArgs e)
        {
            choosenLine = (sender as FrameLayout).Tag.ToString();
            //var lineLength = (sender as FrameLayout).Height;
            var lineLayout = (sender as FrameLayout);
            workList = new List<int[]>(lineLayout.ChildCount);
            var length = schedule.Count;
            for (int i = 0; i < length; i++)
            {
                var otherworkview = lineLayout.FindViewWithTag($"work{i}");
                if (otherworkview != null)
                {
                    var param = otherworkview.LayoutParameters as ViewGroup.MarginLayoutParams;
                    var startPoint = param.TopMargin;
                    workList.Add(new int[2] { startPoint, startPoint + otherworkview.Height });
                }
            }
            if (workList != null && workList.Count != 0)
                if (IsCollising(new int[2] { GetMarginTop(), GetMarginTop() + MinToDp(workInterval) }, workList))
                {
                    AlertDialogs.SimpleAlertDialog("Пересечение", this.Context).Show();
                    return;
                }

            if (acceptTime.Visibility == ViewStates.Gone)
                acceptTime.Visibility = ViewStates.Visible;
            var view = (sender as FrameLayout).FindViewWithTag("work");
            if (view != null)
                return;
            for (int i = 0; i < gridContainer.ChildCount; i++)
            {
                if (gridContainer.GetChildAt(i) is FrameLayout)
                {
                    view = gridContainer.GetChildAt(i).FindViewWithTag("work");
                    if (view != null)
                        (gridContainer.GetChildAt(i) as FrameLayout).RemoveView(view);
                }
            }

            var workView = new TextView((sender as FrameLayout).Context);
            workView.Tag = "work";
            workView.SetBackgroundColor(Color.GreenYellow);
            var parms = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, MinToDp(workInterval));
            parms.TopMargin = GetMarginTop();
            workView.LayoutParameters = parms;
            workView.Text = $"Заказ-наряд\n№{orderId}\nДлительность: {workInterval} мин.";
            workView.Touch += WorkView_Touch;
            (sender as FrameLayout).AddView(workView);
        }


        private int prevY;
        private void WorkView_Touch(object sender, View.TouchEventArgs e)
        {
            var view = sender as TextView;
            var par = view.LayoutParameters as FrameLayout.LayoutParams;
            switch (e.Event.Action)
            {
                case MotionEventActions.Move:
                    par.TopMargin += (int)e.Event.RawY - prevY;
                    if (par.TopMargin < GetMarginTop())
                        break;
                    if (workList != null && workList.Count != 0)
                        if (IsCollising(new int[2] { par.TopMargin, par.TopMargin + MinToDp(workInterval) }, workList))
                            break;
                    prevY = (int)e.Event.RawY;
                    view.LayoutParameters = par;
                    break;
                case MotionEventActions.Up:
                    par.TopMargin += (int)e.Event.RawY - prevY;
                    view.LayoutParameters = par;
                    scrollViewLayout.RequestDisallowInterceptTouchEvent(false);
                    break;
                case MotionEventActions.Down:                    
                    prevY = (int)e.Event.RawY;
                    scrollViewLayout.RequestDisallowInterceptTouchEvent(true);
                    break;
            }
        }

        private void DrawTimeIntervalsAndHorizonTalDividers()
        {
            var hour = workStart.Hour;
            var minutes = workStart.Minute.ToString();
            TimeIntervalItemHeight = MinToDp(30); //GlobalLayoutHeight / (stoDayWorkLength * 2);
            for (int r = 0; r < ROWS; r++)
            {
                var time = new TextView(timeIntervalGrid.Context);
                time.SetTextColor(Color.Black);
                LinearLayout.LayoutParams timeparam;
                timeparam = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, TimeIntervalItemHeight);
                //timeparam.TopMargin = 4;
                time.LayoutParameters = timeparam;
                hour = r == 0 ? hour : minutes == "30" ? hour + 1 : hour;
                minutes = r == 0 ? minutes : minutes == "30" ? "00" : "30";
                time.Text = $"{hour}:{minutes}";
                time.Gravity = GravityFlags.Top;
                timeIntervalGrid.AddView(time);

                //Drawing horizotnal dividers
                var devider = new View(globalLayout.Context);
                devider.SetBackgroundColor(Color.Black);
                FrameLayout.LayoutParams deviderparam = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 1);
                deviderparam.TopMargin = r * TimeIntervalItemHeight;
                devider.LayoutParameters = deviderparam;
                globalLayout.AddView(devider);
            }
        }

        private void DrawWorkLines()
        {
            for (int i = 0; i < lineCount; i++)
            {
                var lineContainer = new FrameLayout(gridContainer.Context);
                lineContainer.Tag = i.ToString();
                lineContainer.Click += LineContainer_Click;
                lineContainer.SetBackgroundColor(Color.Rgb(200, 200, 200));
                var parms = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                parms.Weight = 1;
                lineContainer.LayoutParameters = parms;
                gridContainer.AddView(lineContainer);

                var deviderLine = new View(gridContainer.Context);
                var deviderLineParams = new LinearLayout.LayoutParams(1, ViewGroup.LayoutParams.MatchParent);
                deviderLine.LayoutParameters = deviderLineParams;
                deviderLine.SetBackgroundColor(Color.DimGray);
                gridContainer.AddView(deviderLine);
            }
        }

        private void DrawCurrentTimeLine()
        {
            var currentTimeLine = scheduleContainer.FindViewWithTag("currentTimeLine");
            if (currentTimeLine != null)
            {
                var parameters = currentTimeLine.LayoutParameters as FrameLayout.LayoutParams;
                parameters.TopMargin = GetMarginTop();
                currentTimeLine.LayoutParameters = parameters;
            }
            else
            {
                currentTimeLine = new View(gridContainer.Context);
                currentTimeLine.Tag = "currentTimeLine";
                currentTimeLine.SetBackgroundColor(Color.Black);
                var parameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 4);
                parameters.TopMargin = GetMarginTop();
                currentTimeLine.LayoutParameters = parameters;
                scheduleContainer.AddView(currentTimeLine);
            }
            //while (!token.IsCancellationRequested)
            //{
            //    var currentTimeLine = scheduleContainer.FindViewWithTag("currentTimeLine");
            //    if (currentTimeLine != null)
            //    {
            //        var parameters = currentTimeLine.LayoutParameters as FrameLayout.LayoutParams;
            //        parameters.TopMargin = GetMarginTop();
            //        currentTimeLine.LayoutParameters = parameters;
            //    }
            //    else
            //    {
            //        currentTimeLine = new View(gridContainer.Context);
            //        currentTimeLine.Tag = "currentTimeLine";
            //        currentTimeLine.SetBackgroundColor(Color.Black);
            //        var parameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 4);
            //        parameters.TopMargin = GetMarginTop();
            //        currentTimeLine.LayoutParameters = parameters;
            //        scheduleContainer.AddView(currentTimeLine);
            //    }                
            //    await Task.Delay(interval, token);
            //}
        }

        private int GetMarginTop()
        {
            var now = DateTime.Now;
            var value = MinToDp((now.Hour - workStart.Hour) * 60 + now.Minute - workStart.Minute);
            return value;
        }

        private int MinToDp(int minutes)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, minutes * 1.7f, Resources.DisplayMetrics);
        }

        private void GetElements(View view)
        {
            selectedDate = view.FindViewById<TextView>(Resource.Id.chooseordertime_date);
            switchDateLeft = view.FindViewById<ImageView>(Resource.Id.chooseordertime_date_left);
            switchDateRight = view.FindViewById<ImageView>(Resource.Id.chooseordertime_date_right);
            timeIntervalGrid = view.FindViewById<LinearLayout>(Resource.Id.chooseordertime_timeintervaltable);
            gridContainer = view.FindViewById<LinearLayout>(Resource.Id.chooseordertime_line_container);
            scheduleContainer = view.FindViewById<FrameLayout>(Resource.Id.chooseordertime_schedule_container);
            globalLayout = view.FindViewById<FrameLayout>(Resource.Id.chooseordertime_globalcontainer);
            acceptTime = view.FindViewById<Button>(Resource.Id.chooseordertime_accept_button);
            scrollViewLayout = view.FindViewById<ScrollView>(Resource.Id.chooseordertime_scrollview_layout);
        }
    }
}