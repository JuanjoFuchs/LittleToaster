using System;
using System.Windows.Forms;

namespace LittleToaster
{
  public partial class Toaster : Form {
    const int AnimationInterval = 10;
    const int MovementDelta = 5;
    const int HoldInterval = 20;
    
    readonly Timer _appearTimer;
    readonly Timer _disappearTimer;
    readonly Timer _holdTimer;
    
    int _startPositionX;
    int _startPositionY;

    int _hold;

    public Toaster(string message) {
      InitializeComponent();
      
      TopMost = true;
      ShowInTaskbar = false;

      if (!string.IsNullOrWhiteSpace(message)) {
        messageLabel.Text = message;
      }

      _appearTimer = new Timer { Interval = AnimationInterval };
      _appearTimer.Tick += AppearTimerOnTick;

      _disappearTimer = new Timer { Interval = AnimationInterval };
      _disappearTimer.Tick += DisappearTimerOnTick;

      _holdTimer = new Timer { Interval = HoldInterval };
      _holdTimer.Tick += HoldTimerOnTick;
    }

    bool IsWindowCompletelyVisible {
      get {
        return _startPositionY < WorkingAreaHeigh - Height;
      }
    }

    bool IsWindowCompletelyHidden {
      get {
        return _startPositionY > WorkingAreaHeigh;
      }
    }

    int WorkingAreaWidth {
      get {
        return Screen.PrimaryScreen.WorkingArea.Width;
      }
    }

    int WorkingAreaHeigh {
      get {
        return Screen.PrimaryScreen.WorkingArea.Height;
      }
    }

    void PositionWindow() {
      _startPositionX = WorkingAreaWidth - Width;
      _startPositionY = WorkingAreaHeigh;
      SetDesktopLocation(_startPositionX, _startPositionY);

      _appearTimer.Start();
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      PositionWindow();
    }

    void AppearTimerOnTick(object sender, EventArgs eventArgs) {
      _startPositionY -= MovementDelta;
      if (IsWindowCompletelyVisible) {
        _appearTimer.Stop();
        _holdTimer.Start();
      } else {
        SetDesktopLocation(_startPositionX, _startPositionY);
      }
    }

    void HoldTimerOnTick(object sender, EventArgs eventArgs)
    {
      _hold++;
      if (_hold > HoldInterval) {
        _holdTimer.Stop();
        _disappearTimer.Start();
      }
    }

    void DisappearTimerOnTick(object sender, EventArgs eventArgs)
    {
      _startPositionY += MovementDelta;
      if (IsWindowCompletelyHidden) {
        _disappearTimer.Stop();
        Close();
      } else {
        SetDesktopLocation(_startPositionX, _startPositionY);
      }
    }
  }
}
