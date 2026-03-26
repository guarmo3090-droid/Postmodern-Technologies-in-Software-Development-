# my_tail


## Структура проєкту

```
my_tail/
├── my_tail.sln
├── my_tail/
│   ├── my_tail.csproj
│   ├── Program.cs       
│   └── App.cs           
└── my_tail.Tests/
    ├── my_tail.Tests.csproj
    └── AppTests.cs      
```

## Збірка та запуск

```bash

dotnet build my_tail.sln

dotnet run --project my_tail/my_tail.csproj -- -n 5 file.txt

dotnet publish my_tail/my_tail.csproj -c Release -r win-x64 --self-contained true -o ./publish

dotnet test my_tail.sln
```

## Специфікація

### Синтаксис

```
my_tail [-n N] [file ...]
```

| Аргумент | Опис |
|----------|------|
| `-n N`   |  |
| `file`   |  |
| `-`      |  |

### Приклади

```bash
my_tail file.txt

my_tail -n 3 file.txt

echo -e "a\nb\nc\nd\ne" | my_tail -n 2

my_tail -n 5 file1.txt file2.txt

my_tail -n5 file.txt
```

### Exit codes

| Код | Значення |
|-----|----------|
| `0` | Успішне виконання |
| `1` | Часткова помилка  |
| `2` | Неправильні аргументи |

### Поведінка при кількох файлах

## Архітектурне рішення



```csharp
public static int Run(string[] args, TextReader stdin, TextWriter stdout, TextWriter stderr)
```

```csharp
var input  = new StringReader("a\nb\nc\n");
var output = new StringWriter();
var error  = new StringWriter();

int code = App.Run(new[] { "-n", "2" }, input, output, error);

Assert.That(code,          Is.EqualTo(0));
Assert.That(output.ToString(), Is.EqualTo("b\nc\n"));
Assert.That(error.ToString(),  Is.Empty);
```
