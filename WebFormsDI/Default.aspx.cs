namespace WebFormsDI
{
    using System;
    using System.Drawing;
    using System.Web.UI;

    public interface I_DefaultView
    {
        event EventHandler ButtonClicked;

        void SetButtonColorRed();
    }

    public partial class _Default : Page, I_DefaultView
    {
        //[Dependency]
        //public _DefaultPresenter Presenter { get; set; }

        public event EventHandler ButtonClicked;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Button1.Click += this.ButtonClicked;
        }

        public void SetButtonColorRed()
        {
            this.Button1.BackColor = Color.Red;
        }
    }
}