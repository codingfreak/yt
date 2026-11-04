namespace codingfreaks.Countdown.Ui.WebApp.Components.Pages
{
    using System.Globalization;

    using Humanizer;

    /// <summary>
    /// Represents the home view of the application.
    /// </summary>
    public partial class Home : IAsyncDisposable
    {
        #region member vars

        /// <summary>
        /// Cancels the ticking timer when the component is disposed.
        /// </summary>
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        #endregion

        #region explicit interfaces

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            await _cancellationTokenSource.CancelAsync();
            _cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

        #region methods

        /// <inheritdoc />
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                // Starts only once the circuit is interactive (not during prerendering).
                _ = RunTimerAsync();
            }
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            Logger.LogInformation("Homepage called.");
            UpdateRemaining();
        }

        /// <summary>
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetUnitLabel(TimeUnit unit, int value)
        {
            var text = Configurator.Formatters.ResolveForCulture(Culture)
                .TimeSpanHumanize(unit, value);
            return text.Split(' ')
                .Last();
        }

        /// <summary>
        /// Ticks every second in an endless loop and re-renders until the component is disposed.
        /// </summary>
        private async Task RunTimerAsync()
        {
            using (var timer = new PeriodicTimer(TimeSpan.FromSeconds(1)))
            {
                try
                {
                    while (await timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
                    {
                        UpdateRemaining();
                        await InvokeAsync(StateHasChanged);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Component disposed.
                }
            }
        }

        /// <summary>
        /// Recalculates <see cref="Remaning" />.
        /// </summary>
        private void UpdateRemaining()
        {
            var remaining = TargetDate - DateTime.Now;
            Remaning = remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }

        #endregion

        #region properties

        /// <summary>
        /// The date and time we are approaching to.
        /// </summary>
        /// <remarks>
        /// Always defaults to the last micro-second of the current year.
        /// </remarks>
        private DateTime TargetDate { get; } = new(DateTime.Now.Year, 12, 31, 23, 59, 59, 999);

        /// <summary>
        /// The remaining time until we reach the <see cref="TargetDate" />.
        /// </summary>
        private TimeSpan Remaning { get; set; }

        /// <summary>
        /// The relative humanized duration text.
        /// </summary>
        private string RelativeText => TargetDate.Humanize(false, culture: Culture);

        /// <summary>
        /// The culture to humanize to.
        /// </summary>
        private CultureInfo Culture { get; set; } = Thread.CurrentThread.CurrentUICulture;

        /// <summary>
        /// The time parts to render.
        /// </summary>
        /// <remarks>
        /// This is using yield returns so that the rendering can happen fast.
        /// </remarks>
        private IEnumerable<(int Value, string Label)> TimeParts
        {
            get
            {
                yield return (Remaning.Days, GetUnitLabel(TimeUnit.Day, Remaning.Days));
                yield return (Remaning.Hours, GetUnitLabel(TimeUnit.Hour, Remaning.Hours));
                yield return (Remaning.Minutes, GetUnitLabel(TimeUnit.Minute, Remaning.Minutes));
                yield return (Remaning.Seconds, GetUnitLabel(TimeUnit.Minute, Remaning.Seconds));
            }
        }

        #endregion
    }
}
