using Xunit;
using Bunit;
using BlazorConf2021.Client.Pages;

namespace BlazorConf2021.Test
{
    public class CounterTest
    {

        [Fact]
        public void CounterComponentTest()
        {
            // Arrange
            using var ctx = new TestContext();
            var component = ctx.RenderComponent<Counter>();

            // Act
            component.Find("button").Click();

            // Assert
            component.MarkupMatches(@"<h1>Counter</h1>
<p>Current count: 1</p>
<button class=""btn btn-primary"" >Click me</button>");
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void CounterComponent_ClicksTest(int clicks)
        {
            // Arrange
            using var ctx = new TestContext();
            var component = ctx.RenderComponent<Counter>();

            // Act
            for (int i = 0; i < clicks; i++)
            {
                 component.Find("button").Click();
            }

            // Assert
            component.MarkupMatches(@$"<h1>Counter</h1>
<p>Current count: {clicks}</p>
<button class=""btn btn-primary"" >Click me</button>");
        }


        [Fact]
        public void Test_Diff()
        {
            using var ctx = new TestContext();
            var component = ctx.RenderComponent<Counter>();

            var button = component.Find("button");
            button.Click();

            var diff = component.GetChangesSinceFirstRender();
            diff.ShouldHaveSingleTextChange("Current count: 1");
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Test_DiffTheory(int clicks)
        {
            using var ctx = new TestContext();
            var component = ctx.RenderComponent<Counter>();

            var button = component.Find("button");
            for (int i = 0; i < clicks; i++)
            {
                button.Click();    
            }

            var diff = component.GetChangesSinceFirstRender();
            diff.ShouldHaveSingleTextChange($"Current count: {clicks}");
        }
    }
}