using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorConf2021.Client.Pages;
using Bunit;
using Xunit;

namespace Test
{
    public class IndexTest
    {
        [Fact]
        public void ShowMessage()
        {
            using var ctx = new TestContext();
            var component = ctx.RenderComponent<BlazorConf2021.Client.Pages.Index>();

            var button = component.Find("button");
            button.Click();
            component.WaitForAssertion(() =>
                {
                    component.MarkupMatches(@"<h1>Hello, world!</h1>
Welcome to your new app.
<button >ShowMessage</button>");
                },
                TimeSpan.FromSeconds(1));
        }
    }
}
