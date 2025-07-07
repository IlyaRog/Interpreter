Pascal Interpreter in C# (ANTLR 4)
❗**Status: Completed** — This project is no longer under active development.

This project is a working interpreter for a simplified subset of the Pascal programming language. It is written in C# and built using [ANTLR 4](https://www.antlr.org/) for lexical and syntactic analysis.

💡 Features

- Variable declarations and assignments
- Integer arithmetic (`+`, `-`, `*`, `div`, `mod`)
- Conditional statements (`if ... then ... else`)
- Loops (`while ... do`)
- Console input (`read(...)`) and output (`write(...)`)
- Support for string literals in output

🔧 Technologies

- C# (.NET 8)
- ANTLR 4
- Pascal grammar from [antlr/grammars-v4](https://github.com/antlr/grammars-v4)

  //example.pas must be located in the same directory as the executable file (e.g., .exe if you're using Windows).
📁 Example (Pascal)

program Calculator;
var
  a, b, result: integer;
begin
  read(a);
  read(b);

  result := a + b;
  write('result +: ');
  write(result);

  result := a - b;
  write('result -: ');
  write(result);

  result := a * b;
  write('result *: ');
  write(result);

  if b <> 0 then
  begin
    result := a div b;
    write('result /: ');
    write(result);
    result := a mod b;
    write('result %: ');
    write(result);
  end
  else
  begin
    write('Division by zero is not possible');
  end;
end.
