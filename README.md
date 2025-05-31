# tic-tac-toe.cs

An implementation of a tic-tac-toe program in C#. See [goldenstein64/tic-tac-toe.spec](https://github.com/goldenstein64/tic-tac-toe.spec) for the design specification.

## Usage

```sh
dotnet run --project=App
```

## Building

```sh
dotnet build --configuration=Release
# output can be found in ./App/bin/Release/net8.0
```

## Testing

```sh
dotnet test
```

## Publishing

```sh
# publish core
dotnet publish Lib

# publish console
dotnet publish App
```
