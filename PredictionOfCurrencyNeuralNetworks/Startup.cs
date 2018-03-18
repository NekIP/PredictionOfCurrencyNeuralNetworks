using Business;
using DataBase.Repositories;
using DataManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PredictionOfCurrencyNeuralNetworks {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();
            InitCollectors(services);
            InitRepositories(services);
            InitBuisness(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        protected void InitBuisness(IServiceCollection services) {
            services.AddTransient<IPredictionOfCurrencyUsdToRub, PredictionOfCurrencyUsdToRub>();
        }

        protected void InitCollectors(IServiceCollection services) {
            services.AddTransient<IUsdToRubCurrencyCollector, UsdToRubCurrencyCollector>();
            services.AddTransient<ICAC40Collector, CAC40Collector>();
            services.AddTransient<ICSI200Collector, CSI200Collector>();
            services.AddTransient<IDataForNeuralNetworkCollector, DataForNeuralNetworkCollector>();
            services.AddTransient<IDowJonesCollector, DowJonesCollector>();
            services.AddTransient<IGdpPerCapitaPppCollector, GdpPerCapitaPppCollector>();
            services.AddTransient<IGoldCollector, GoldCollector>();
            services.AddTransient<IInflationCollector, InflationCollector>();
            services.AddTransient<IMMVBCollector, MMVBCollector>();
            services.AddTransient<IOliBrentCollector, OliBrentCollector>();
            services.AddTransient<IOliLightCollector, OliLightCollector>();
            services.AddTransient<IRefinancingRateCollector, RefinancingRateCollector>();
            services.AddTransient<IRTSCollector, RTSCollector>();
            services.AddTransient<ISAndP500Collector, SAndP500Collector>();
            services.AddTransient<ITradeBalanceCollector, TradeBalanceCollector>();
        }

        protected void InitRepositories(IServiceCollection services) {
            services.AddTransient<IUsdToRubCurrencyRepository, UsdToRubCurrencyRepository>();
            services.AddTransient<ICAC40Repository, CAC40Repository>();
            services.AddTransient<ICSI200Repository, CSI200Repository>();
            services.AddTransient<IDataForNeuralNetworkRepository, DataForNeuralNetworkRepository>();
            services.AddTransient<IDowJonesRepository, DowJonesRepository>();
            services.AddTransient<IGdpPerCapitaPppRepository, GdpPerCapitaPppRepository>();
            services.AddTransient<IGoldRepository, GoldRepository>();
            services.AddTransient<IInflationRepository, InflationRepository>();
            services.AddTransient<IMMVBRepository, MMVBRepository>();
            services.AddTransient<IOliBrentRepository, OliBrentRepository>();
            services.AddTransient<IOliLightRepository, OliLightRepository>();
            services.AddTransient<IRefinancingRateRepository, RefinancingRateRepository>();
            services.AddTransient<IRTSRepository, RTSRepository>();
            services.AddTransient<ISAndPRepository, SAndPRepository>();
            services.AddTransient<ITradeBalanceRepository, TradeBalanceRepository>();
        }
    }
}
