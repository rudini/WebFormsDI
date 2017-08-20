namespace WebFormsDI.Presenter
{
    using System;

    using WebFormsDI.Models;

    public class _DefaultPresenter
    {
        private I_DefaultView view;

        private readonly Service service;

        public _DefaultPresenter(I_DefaultView view, Service service)
        {
            this.view = view;
            this.view.ButtonClicked += this.ButtonClicked;
            this.service = service;
        }

        private void ButtonClicked(object sender, EventArgs eventArgs)
        {
            this.view.SetButtonColorRed();
        }
    }
}