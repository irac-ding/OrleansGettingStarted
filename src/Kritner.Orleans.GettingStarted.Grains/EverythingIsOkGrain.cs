using Kritner.Orleans.GettingStarted.GrainInterfaces;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Kritner.Orleans.GettingStarted.Grains
{
    [StorageProvider(ProviderName = Constants.OrleansMemoryProvider)]
    public class EverythingIsOkGrain : Grain, IEverythingIsOkGrain
    {
        IGrainReminder _reminder = null;

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            // Grain-ception!
            var emailSenderGrain = GrainFactory
                .GetGrain<IEmailSenderGrain>(Guid.Empty);

            await emailSenderGrain.SendEmail(
                "homer@anykey.com",
                new[] 
                {
                    "marge@anykey.com",
                    "bart@anykey.com",
                    "lisa@anykey.com",
                    "maggie@anykey.com"
                },
                "Everything's ok!",
                $"This is from reminder:{this.GetPrimaryKeyString()}, This alarm will sound every 1 minute, as long as everything is ok!"
            );
            ////when ReceiveReminder we kppp activ the gain and update the reminder to break the period at least 1 minute limit.
            //string keyGain = $"{nameof(IEverythingIsOkGrain)}-{Guid.NewGuid()}";
            //var grain = GrainFactory.GetGrain<IEverythingIsOkGrain>(keyGain);
            //await RegisterOrUpdateReminder(
            //    keyGain,
            //    TimeSpan.FromSeconds(3),
            //    TimeSpan.FromMinutes(1) // apparently the minimum
            //);

            //when ReceiveReminder we kppp active a new gain and register the reminder.
            //string keyGain = $"{nameof(IEverythingIsOkGrain)}-{Guid.NewGuid()}";
            //var grain = GrainFactory.GetGrain<IEverythingIsOkGrain>(keyGain);
            //await grain.Start();
        }

        public async Task Start()
        {
            if (_reminder != null)
            {
                return;
            }

            _reminder = await RegisterOrUpdateReminder(
                this.GetPrimaryKeyString(),
                TimeSpan.FromSeconds(3),
                TimeSpan.FromMinutes(1) // apparently the minimum
            );
            //RegisterTimer((obj)=>
            //{
            //    var emailSenderGrain = GrainFactory.GetGrain<IEmailSenderGrain>(Guid.Empty);
            //    emailSenderGrain.SendEmail(
            //        "homer@anykey.com",
            //        new[]
            //        {
            //        "marge@anykey.com",
            //        "bart@anykey.com",
            //        "lisa@anykey.com",
            //        "maggie@anykey.com"
            //        },
            //        "Everything's ok!",
            //        "This is from timer, This alarm will sound every 1 second, as long as everything is ok!"
            //    );
            //    return Task.CompletedTask;
            //}, this.GetPrimaryKeyString(), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(1));
        }
        
        public async Task Stop()
        {
            await UnregisterReminder(_reminder);
            _reminder = null;
        }
    }
}
