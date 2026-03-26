# my_tail

Реалізація утиліти `tail` на C# (.NET 8).

## Структура проєкту

```
my_tail/
├── my_tail.sln
├── my_tail/
│   ├── my_tail.csproj
│   ├── Program.cs       ← тонкий entry point
│   └── App.cs           ← уся логіка (тестується окремо)
└── my_tail.Tests/
    ├── my_tail.Tests.csproj
    └── AppTests.cs      ← NUnit тести
```

## Збірка та запуск

```bash
# Зібрати
dotnet build my_tail.sln

# Запустити
dotnet run --project my_tail/my_tail.csproj -- -n 5 file.txt

# Або опублікувати як exe
dotnet publish my_tail/my_tail.csproj -c Release -r win-x64 --self-contained true -o ./publish
# → publish/my_tail.exe

# Запустити тести
dotnet test my_tail.sln
```

## Специфікація

### Синтаксис

```
my_tail [-n N] [file ...]
```

| Аргумент | Опис |
|----------|------|
| `-n N`   | Кількість останніх рядків для виведення (за замовчуванням: 10) |
| `file`   | Один або кілька файлів. Якщо не вказано — читає зі stdin |
| `-`      | Явне читання зі stdin |

### Приклади

```bash
# Останні 10 рядків файлу (за замовчуванням)
my_tail file.txt

# Останні 3 рядки
my_tail -n 3 file.txt

# Зі stdin
echo -e "a\nb\nc\nd\ne" | my_tail -n 2

# Кілька файлів (друкує заголовки ==> file <==)
my_tail -n 5 file1.txt file2.txt

# Компактна форма прапорця
my_tail -n5 file.txt
```

### Exit codes

| Код | Значення |
|-----|----------|
| `0` | Успішне виконання |
| `1` | Часткова помилка (напр. файл не знайдено) |
| `2` | Неправильні аргументи |

### Поведінка при кількох файлах

Якщо передано більше одного файлу, перед кожним виводиться заголовок:
```
==> file1.txt <==
...вміст...

==> file2.txt <==
...вміст...
```

## Архітектурне рішення

Логіка винесена у статичний клас `App` з методом:

```csharp
public static int Run(string[] args, TextReader stdin, TextWriter stdout, TextWriter stderr)
```

Це дозволяє тестувати програму без запуску процесу:

```csharp
var input  = new StringReader("a\nb\nc\n");
var output = new StringWriter();
var error  = new StringWriter();

int code = App.Run(new[] { "-n", "2" }, input, output, error);

Assert.That(code,          Is.EqualTo(0));
Assert.That(output.ToString(), Is.EqualTo("b\nc\n"));
Assert.That(error.ToString(),  Is.Empty);
```
