using Xunit;
using Bunit;
using BlazorConf2021.Client.Pages;

namespace BlazorConf2021.Test
{
    public class CounterTest
    {
        [Fact]
        public void CounterComponent_Load_Test()
        {
            using var ctx = new TestContext();
            var component = ctx.RenderComponent<Counter>();

            component.MarkupMatches(@"<h1>Counter</h1>
<p>Current count: 0</p>
<button class=""btn btn-primary"" >Click me</button>");
        }

        [Fact]
        public void CounterComponent_ClickTest()
        {
            using var ctx = new TestContext();
            var component = ctx.RenderComponent<Counter>();
            
            component.Find("button").Click();

            component.MarkupMatches(@"<h1>Counter</h1>
<p>Current count: 1</p>
<button class=""btn btn-primary"" >Click me</button>");
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void CounterComponent_NClicksTest(int clicks)
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


        [Fact]
        public void Test_DiffWithParameter()
        {
            using var ctx = new TestContext();
            var component = ctx.RenderComponent<Counter>(
                (nameof(Counter.currentCount),5));

            var button = component.Find("button");
            button.Click();

            var diff = component.GetChangesSinceFirstRender();
            diff.ShouldHaveSingleTextChange("Current count: 6");
        }

        [Fact]
        public void Test_DiffWithParameterAndCallback()
        {
            using var ctx = new TestContext();
            var callbackValue = 0;
            var component = ctx.RenderComponent<Counter>(
                ComponentParameterFactory.Parameter(nameof(Counter.currentCount), 5),
                ComponentParameterFactory.EventCallback<int>(nameof(Counter.OnChangeCount), (int value) => { callbackValue = value; })
            );
                var button = component.Find("button");
            button.Click();

            var diff = component.GetChangesSinceFirstRender();
            diff.ShouldHaveSingleTextChange("Current count: 6");

            Assert.Equal(6, callbackValue);
        }
    }
}