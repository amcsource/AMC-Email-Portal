using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

/// <summary>
/// Summary description for JobSchedular
/// </summary>
public class JobSchedular
{
	public JobSchedular()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void Start()
    {
        //1. Scheduler for sql email
        ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
        IScheduler scheduler = schedulerFactory.GetScheduler();
        scheduler.Start();
        IJobDetail jobDetailsForEmailEvery5Minute = JobBuilder.Create<EmailJob>()
            .WithIdentity("EmailEvery5Minute", "Group1").Build();

        //Daily
        ITrigger triggerForEmailEvery5Minute = TriggerBuilder.Create().WithIdentity("EmailEvery5Minute", "Group1").StartNow()
            .WithDailyTimeIntervalSchedule
              (s =>
                s.WithIntervalInMinutes(5)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(7, 0))
              )
            .Build();


        //Call Schedular
        scheduler.ScheduleJob(jobDetailsForEmailEvery5Minute, triggerForEmailEvery5Minute);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //2. Scheduler for pharmacy worksheet
        ISchedulerFactory schedulerFactoryWorksheet = new StdSchedulerFactory();
        IScheduler schedulerWorksheet = schedulerFactoryWorksheet.GetScheduler();
        schedulerWorksheet.Start();
        IJobDetail jobDetailsForEmailEvery5MinuteWorksheet = JobBuilder.Create<EmailPharmacyWorksheet>()
            .WithIdentity("EmailEvery5MinuteWorksheet", "Group2").Build();

        //Daily
        ITrigger triggerForEmailEvery5MinuteWorksheet = TriggerBuilder.Create().WithIdentity("EmailEvery5MinuteWorksheet", "Group2").StartNow()
            .WithDailyTimeIntervalSchedule
              (s =>
                s.WithIntervalInMinutes(5)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(7, 0))
              )
            .Build();


        //Call Schedular
        schedulerWorksheet.ScheduleJob(jobDetailsForEmailEvery5MinuteWorksheet, triggerForEmailEvery5MinuteWorksheet);


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //3. Scheduler for excel job
        ISchedulerFactory schedulerFactoryExcel = new StdSchedulerFactory();
        IScheduler schedulerExcel = schedulerFactoryExcel.GetScheduler();
        schedulerExcel.Start();
        IJobDetail jobDetailsForEmailEvery5MinuteExcel = JobBuilder.Create<ExcelJob>()
            .WithIdentity("EmailEvery5MinuteExcel", "Group2").Build();

        //Daily
        ITrigger triggerForEmailEvery5MinuteExcel = TriggerBuilder.Create().WithIdentity("EmailEvery5MinuteExcel", "Group2").StartNow()
            .WithDailyTimeIntervalSchedule
              (s =>
                s.WithIntervalInMinutes(5)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(7, 0))
              )
            .Build();


        //Call Schedular
        schedulerExcel.ScheduleJob(jobDetailsForEmailEvery5MinuteExcel, triggerForEmailEvery5MinuteExcel);


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //4. Scheduler for print job
        ISchedulerFactory schedulerFactoryPrint = new StdSchedulerFactory();
        IScheduler schedulerPrint = schedulerFactoryPrint.GetScheduler();
        schedulerPrint.Start();
        IJobDetail jobDetailsForEmailEvery5MinutePrint = JobBuilder.Create<PrintJob>()
            .WithIdentity("EmailEvery5MinutePrint", "Group2").Build();

        //Daily
        ITrigger triggerForEmailEvery5MinutePrint = TriggerBuilder.Create().WithIdentity("EmailEvery5MinutePrint", "Group2").StartNow()
            .WithDailyTimeIntervalSchedule
              (s =>
                s.WithIntervalInMinutes(5)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(7, 0))
              )
            .Build();


        //Call Schedular
        schedulerPrint.ScheduleJob(jobDetailsForEmailEvery5MinutePrint, triggerForEmailEvery5MinutePrint);
    }
}