
using System;
using NUnit.Framework;
using CocosNet;

namespace CocosNetTests {

	[TestFixture]
	public class SchedulerTests {

		[SetUp]
		public void Setup() {
			Scheduler.Instance.UnscheduleAll();
		}

		[Test]
		public void ScheduleTimersByPriority() {
			Scheduler.Timer first = new Scheduler.Timer(-1);
			Scheduler.Timer second = new Scheduler.Timer(3);
			Scheduler.Timer third = new Scheduler.Timer(6);
			
			bool firstTicked = false;
			bool secondTicked = false;
			bool thirdTicked = false;
			
			first.Tick += delegate {
				Assert.IsFalse(secondTicked, "second ticked before first");
				Assert.IsFalse(thirdTicked, "third ticked before first");	
				Assert.IsFalse(firstTicked, "First already ticked");
				firstTicked = true;
			};
			
			second.Tick += delegate {
				Assert.IsTrue(firstTicked, "First should tick before second");
				Assert.IsFalse(thirdTicked, "third ticked before second");
				Assert.IsFalse(secondTicked, "Second already ticked");
				secondTicked = true;
			};
			
			third.Tick += delegate {
				Assert.IsTrue(firstTicked, "First should tick before third");
				Assert.IsTrue(secondTicked, "Second should tick before third");
				Assert.IsFalse(thirdTicked, "Third already ticked");
				thirdTicked = true;
			};
			
			// schedule them out of order, they should get sorted by priority
			Scheduler.Instance.Schedule(second);
			Scheduler.Instance.Schedule(first);
			Scheduler.Instance.Schedule(third);
			
			Scheduler.Instance.OnTick(0.5f);
			
			Assert.IsTrue(firstTicked, "First never ticked");
			Assert.IsTrue(secondTicked, "Second never ticked");
			Assert.IsTrue(thirdTicked, "Third never ticked");
		}

		[Test]
		public void EnsureTimersOnlyTickAtTheirIntervals() {
			Scheduler.Timer alwaysTicks = new Scheduler.Timer();
			Scheduler.Timer ticksEachSecond = new Scheduler.Timer(0, 1);
			
			int alwaysTickCount = 0;
			int ticksEachSecondCount = 0;
			
			alwaysTicks.Tick += delegate { ++alwaysTickCount; };
			
			ticksEachSecond.Tick += delegate { ++ticksEachSecondCount; };
			
			Scheduler.Instance.Schedule(alwaysTicks);
			Scheduler.Instance.Schedule(ticksEachSecond);
			
			const float TickInterval = 0.25f;
			const int NumTicks = 16;
			for (int i = 0; i < NumTicks; ++i) {
				Scheduler.Instance.OnTick(TickInterval);
			}
			
			Assert.AreEqual(NumTicks, alwaysTickCount, "alwaysTicks did not tick on each call");
			Assert.AreEqual((int)(NumTicks * TickInterval), ticksEachSecondCount, "ticksEachSecond did not tick as expected");
		}

		[Test]
		public void UnscheduleTimer() {
			Scheduler.Timer always = new Scheduler.Timer();
			Scheduler.Timer removeMe = new Scheduler.Timer();
			
			int alwaysCount = 0;
			int removeMeCount = 0;
			
			always.Tick += delegate { ++alwaysCount; };
			
			removeMe.Tick += delegate { ++removeMeCount; };
			
			Scheduler.Instance.Schedule(always);
			Scheduler.Instance.Schedule(removeMe);
			
			const int TickCount = 5;
			for (int i = 0; i < TickCount; ++i) {
				Scheduler.Instance.OnTick(0.5f);
			}
			Scheduler.Instance.Unschedule(removeMe);
			for (int i = 0; i < TickCount; ++i) {
				Scheduler.Instance.OnTick(0.5f);
			}
			
			Assert.AreEqual(TickCount * 2, alwaysCount, "Always did not tick each time");
			Assert.AreEqual(TickCount, removeMeCount, "Unexpected number of ticks for removeMe");
		}
	}
}
