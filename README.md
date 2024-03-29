# Require Expressions for .Net

![build](https://github.com/andrej-dyck/template-gradle-kotlin/actions/workflows/gradle-ci.yml/badge.svg?branch=main)
[![codecov](https://codecov.io/gh/andrej-dyck/dotnet-extensions-require/branch/main/graph/badge.svg?token=9IL6K5CX37)](https://codecov.io/gh/andrej-dyck/dotnet-extensions-require)
[![NuGet](https://badgen.net/nuget/v/Require-Expressions)](https://www.nuget.org/packages/Require-Expressions/)

A small library that provides _pre-condition_ checks as an extension method on types.
This allows the client code to check a condition on arguments and use the value directly if no exception is thrown.

```csharp
Reservation Request(DateTime now, DateTime reservationDate, int seats) => 
    new Reservation(
        reservationDate.Require(d => d > now, d => $"Reservation date {d} must be in the future"),
        seats.Require(seats > 0, () => "Expected positive number of seats")
    );
```

## NuGet Package

```shell
dotnet add package Require-Expressions
```

## Examples for `Require`

Basic `Require` function

```csharp
var requestedSeats = seats.Require(condition: seats > 0);
// throws ArgumentException with "expected: seats > 0" as message when condition is not met

var requestedSeats = seats.Require(
    condition: seats > 0,
    expectation: "Expected positive number of seats" // or with custom expectation message
);
```

`Require` functions with lazily constructed expectation messages

```csharp
var requestedSeats = seats.Require(
    condition: seats > 0,
    expectation: () => "Expected positive number of seats"
);

var requestedDate = reservationDate.Require(
    condition: reservationDate > now,
    expectation: d => $"Reservation date {d} must be in the future"
);
```

`Require` functions with predicate for convenience

```csharp
var requestedSeats = seats.Require(condition: s => s > 0);
// throws ArgumentException with "expected: s => s > 0" as message when condition is not met

var requestedSeats = seats.Require(
    condition: s => s > 0,
    expectation: "Expected positive number of seats" // or with custom expectation message
);

var requestedDate = reservationDate.Require(
    requirement: d => d > now,
    expectation: d => $"Reservation date {d} must be in the future"
);
```

## Examples for `Check`

For checks other than on arguments, the `Check` function can be used

```csharp
var reservationRequest = JsonSerializer.Deserialize<Reservation>(request)
    .Check(requirement: r => r.Seats > 0) // expectation message will be "expected r => r.Seats > 0"
    .Check(r => r.Date > now, r => $"Reservation date {r.Date} must be in the future")
    // throws CheckFailed with expectation as message when requirement is not satisfied
```

With `Check`, custom exceptions can be constructed

```csharp
var reservationRequest = JsonSerializer.Deserialize<Reservation>(request)
    .Check(r => r.Seats > 0, exception: () => new SeatsMustBePositive())
    .Check(r => r.Date > now, exception: r => new MustBeFutureDateReservateion(r.Date))
```

## Q&A

**Why yet another preconditions library?**

Most libraries I found use precondition _statements_, while I found preconditions as _expressions_ more useful.
For example, the latter allows for [_expression-bodied_ functions](
https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members
).

**I don't like extension methods**

No problem, there are many other great precondition libraries. Have a look
at [.Net's contracts](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.contracts?view=net-6.0).

**I don't want yet another dependency**

No problem, just copy&paste the code you need into your project. Or use the build-in .Net contracts.

**Can you add this function too?**

Probably not as I want to keep this library small and focused. But you are very welcome to post a PR.

## Build & Test

**Build project**

```shell
dotnet build 
```

**Run unit tests**

```shell
dotnet test
```
