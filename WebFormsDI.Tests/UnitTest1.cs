using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebFormsDI.Tests
{
    using FakeItEasy;

    using WebFormsDI.Presenter;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var view = A.Fake<I_DefaultView>();
            var testee = new _DefaultPresenter(view, null);

            view.ButtonClicked += Raise.With(view, EventArgs.Empty);
            
            A.CallTo(() => view.SetButtonColorRed()).MustHaveHappened();
        }
    }
}
