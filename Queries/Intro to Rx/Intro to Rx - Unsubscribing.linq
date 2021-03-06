<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.dll</Reference>
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Joins</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Reactive.Threading.Tasks</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

//http://www.introtorx.com/Content/v1.0.10621.0/03_LifetimeManagement.html#Unsubscribing
var values = new Subject<int>();
var firstSubscription = values.Subscribe(value => Console.WriteLine("1st subscription received {0}", value));
var secondSubscription = values.Subscribe(value => Console.WriteLine("2nd subscription received {0}", value));
values.OnNext(0);
values.OnNext(1);
values.OnNext(2);
values.OnNext(3);
firstSubscription.Dispose();
Console.WriteLine("Disposed of 1st subscription");
values.OnNext(4);
values.OnNext(5);