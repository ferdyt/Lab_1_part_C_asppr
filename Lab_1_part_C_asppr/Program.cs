using Lab_1_part_C_asppr;

int choice = 0;
Matrix optimalMatrix = null;

void ShowMatrix(Matrix matrix)
{
    for (int i = 0; i < matrix.Rows; i++)
    {
        for (int j = 0; j < matrix.Columns; j++)
        {
            Console.Write($"{matrix[i, j]:F2} ");
        }
        Console.WriteLine();
    }
}

void ReverseMatrix()
{
    Matrix matrix = InputManager.InputMatrix();

    if (matrix.Rows != matrix.Columns)
    {
        Console.WriteLine("Помилка! Матриця повинна бути квадратною.");
        return;
    }

    InverseMatrixCalculator eliminator = new InverseMatrixCalculator();
    List<Matrix> iterations = eliminator.Eliminate(matrix);

    int iterationsCount = 1;
    foreach (Matrix m in iterations)
    {
        Console.WriteLine($"\nРозв\'язок для {iterationsCount} елементу дiагоналi:");
        ShowMatrix(m);
        iterationsCount++;
    }
}

void SystemSolution()
{
    Matrix matrix = InputManager.InputMatrix();
    double[] constants = InputManager.InputConstants();

    InverseMatrixCalculator eliminator = new InverseMatrixCalculator();
    List<Matrix> iterations = eliminator.Eliminate(matrix);

    SystemCalculator systemCalculator = new SystemCalculator();
    double[] solutions = systemCalculator.Calculate(matrix, constants);

    int iterationsCount = 1;
    foreach (Matrix m in iterations)
    {
        Console.WriteLine($"\nРозв\'язок для {iterationsCount} елементу дiагоналi:");
        ShowMatrix(m);
        iterationsCount++;
    }

    int solutionIndex = 1;
    Console.WriteLine("\nРозв\'язок системи:");
    foreach (double n in solutions)
    {
        Console.WriteLine($"X[{solutionIndex}]: " + n);
        solutionIndex++;
    }
}

void RankMatrix()
{
    Matrix matrix = InputManager.InputMatrix();

    RankCalculator rankCalculator = new RankCalculator();

    List<Matrix> iterations = rankCalculator.CalculateRank(matrix);

    int iterationsCount = 1;
    foreach (Matrix m in iterations)
    {
        Console.WriteLine($"\nРозв\'язок для {iterationsCount} елементу дiагоналi:");
        ShowMatrix(m);
        iterationsCount++;
    }

    Console.WriteLine("\nРанг матрицi: " + matrix.Rank);
}

int FindRowWithNegativeB(Matrix matrix)
{
    for (int i = 0; i < matrix.Rows - 1; i++)
    {
        if (matrix[i, matrix.Columns - 1] < 0)
        {
            return i;
        }
    }
    return -1;
}

int FindNegativeInRow(Matrix matrix, int row)
{
    for (int j = 0; j < matrix.Columns - 1; j++)
    {
        if (matrix[row, j] < 0)
        {
            return j;
        }
    }
    return -1;
}

int FindPositiveInRow(Matrix matrix, int row)
{
    for (int j = 0; j < matrix.Columns - 1; j++)
    {
        if (matrix[row, j] > 0)
        {
            return j;
        }
    }
    return -1;
}

bool IsNegative(Matrix matrix)
{
    for (int i = 0; i < matrix.Rows - 1; i++)
    {
        if (matrix[i, matrix.Columns - 1] < 0)
        {
            return true;
        }
    }
    return false;
}

void SwapHeaders(Matrix matrix, int r, int s)
{
    string temp = matrix.RowHeaders[r];

    matrix.RowHeaders[r] = matrix.ColumnHeaders[s];
    matrix.ColumnHeaders[s] = temp;
}

string GetResultX(Matrix matrix)
{
    int n = matrix.Columns - 1;
    double[] xValues = new double[n];

    for (int i = 0; i < matrix.Rows - 1; i++)
    {
        string header = matrix.RowHeaders[i];

        if (header.Contains("x"))
        {
            string indexStr = header.Replace("x", "").Trim();

            if (int.TryParse(indexStr, out int index))
            {
                if (index >= 1 && index <= n)
                {
                    xValues[index - 1] = matrix[i, matrix.Columns - 1];
                }
            }
        }
    }

    string result = "X = (" + string.Join("; ", xValues.Select(v => v.ToString("F2"))) + ")";
    return result;
}

void PrintMatrix(Matrix matrix)
{
    Console.Write("\t");
    for (int j = 0; j < matrix.Columns; j++)
    {
        Console.Write($"{matrix.ColumnHeaders[j],8}");
    }
    Console.WriteLine();

    for (int i = 0; i < matrix.Rows; i++)
    {
        Console.Write($"{matrix.RowHeaders[i]}\t");
        for (int j = 0; j < matrix.Columns; j++)
        {
            Console.Write($"{matrix[i, j],8:F2}");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

int OptionalMinNotNegative(Matrix matrix, int col)
{
    int r = -1;
    double minValue = double.MaxValue;
    int lastCol = matrix.Columns - 1;

    for (int i = 0; i < matrix.Rows - 1; i++)
    {
        double element = matrix[i, col];
        double freeTerm = matrix[i, lastCol];

        if (element > 0)
        {
            double ratio = freeTerm / element;

            if (ratio < minValue)
            {
                minValue = ratio;
                r = i;
            }
        }
    }

    return r;
}

int MinNotNegative(Matrix matrix, int col)
{
    int r = -1;
    double minValue = double.MaxValue;
    int lastCol = matrix.Columns - 1;

    for (int i = 0; i < matrix.Rows - 1; i++)
    {
        double element = matrix[i, col];
        double freeTerm = matrix[i, lastCol];

        if (element != 0)
        {
            double ratio = freeTerm / element;

            if (ratio >= 0 && ratio < minValue)
            {
                minValue = ratio;
                r = i;
            }
        }
    }

    return r;
}

int FindNegativeInZRow(Matrix matrix)
{
    int lastRow = matrix.Rows - 1;
    for (int j = 0; j < matrix.Columns - 1; j++)
    {
        if (matrix[lastRow, j] < 0) return j;
    }
    return -1;
}

void DeleteZeroRows()
{
    ModifiedMatrixCalculator eliminator = new ModifiedMatrixCalculator();
    Matrix matrix = InputManager.InputMatrix();
    matrix.InitializeHeaders();

    Console.Write("Вкажiть нульовi рядки: ");
    string input = Console.ReadLine();
    string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    int[] zeroRows = new int[parts.Length];

    for (int i = 0; i < parts.Length; i++)
    {
        if (int.TryParse(parts[i], out int rowIndex))
        {
            zeroRows[i] = rowIndex - 1;
        }
        else
        {
            Console.WriteLine($"Помилка! '{parts[i]}' не є числом. Спробуйте ще раз.");
            return;
        }
    }

    for (int i = 0; i < zeroRows.Length; i++)
    {
        if (zeroRows[i] < 0 || zeroRows[i] >= matrix.Rows)
        {
            Console.WriteLine($"Помилка! Рядок {zeroRows[i] + 1} виходить за межi матрицi");
            return;
        }

        if (matrix.RowHeaders[zeroRows[i]] != "Z")
        {
            matrix.RowHeaders[zeroRows[i]] = "0";
        }
        else
        {
            Console.WriteLine("Помилка! Неможливо змінити рядок Z.");
            return;
        }
    }

    int zeroRowIteration = 0;

    while (zeroRows.Count() > zeroRowIteration)
    {
        int column = FindPositiveInRow(matrix, zeroRows[zeroRowIteration]);

        int r = MinNotNegative(matrix, column);

        eliminator.Calculate(matrix, r, column);
        SwapHeaders(matrix, r, column);

        if (matrix.ColumnHeaders[column] == "0")
        {
            matrix = matrix.FilterColumn(column);
            zeroRowIteration++;
        }
        Console.WriteLine($"\nПромiжна таблиця: (елемент {matrix.ColumnHeaders[column]}, {matrix.RowHeaders[r]})");
        PrintMatrix(matrix);
    }
}

Matrix FindOptimalSolution()
{
    ModifiedMatrixCalculator optimalSolution = new ModifiedMatrixCalculator();

    Matrix matrix = optimalMatrix.Clone();

    while (true)
    {
        int s = FindNegativeInZRow(matrix);

        if (s == -1)
        {
            Console.WriteLine("Оптимальний розв'язок знайдено");
            return matrix;
        }

        int r = OptionalMinNotNegative(matrix, s);

        if (r == -1)
        {
            Console.WriteLine("Цiльова функцiя необмежена ");
            return null;
        }

        matrix = optimalSolution.Calculate(matrix, r, s);

        SwapHeaders(matrix, r, s);

        Console.WriteLine($"\nПромiжна таблиця: (елемент {matrix.ColumnHeaders[s]}, {matrix.RowHeaders[r]} тобто {matrix[r,s]}) ");
        PrintMatrix(matrix);
    }
}

Matrix FindReferenceSolution()
{
    Matrix matrix = InputManager.InputMatrix();
    if (matrix == null) return null;

    matrix.InitializeHeaders();

    ModifiedMatrixCalculator referenceSolution = new ModifiedMatrixCalculator();

    while (IsNegative(matrix))
    {
        int targetRow = FindRowWithNegativeB(matrix);

        if (targetRow == -1)
        {
            break;
        }

        int s = FindNegativeInRow(matrix, targetRow);

        if (s == -1)
        {
            Console.WriteLine("Система обмежень є суперечливою");
            return null;
        }

        int r = MinNotNegative(matrix, s);

        if (r == -1)
        {
            Console.WriteLine("Неможливо знайти розв\'язувальний рядок");
            return null;
        }

        matrix = referenceSolution.Calculate(matrix, r, s);
        SwapHeaders(matrix, r, s);
        Console.WriteLine("\nПромiжна таблиця");
        PrintMatrix(matrix);
    }

    Console.WriteLine("Опорний розв\'язок знайдено");
    return matrix;
}

void ShowReferenceSolution()
{
    Matrix referenceSolution = FindReferenceSolution();

    optimalMatrix = referenceSolution;

    string X = GetResultX(referenceSolution);
    Console.WriteLine("\nОпорний розв\'язок:");
    Console.WriteLine(X);
}

void ShowOptimalSolution()
{
    if (optimalMatrix == null)
    {
        Console.WriteLine("Спочатку знайдіть опорний розв\'язок.");
        return;
    }
    Matrix optimalSolution = FindOptimalSolution();
    if (optimalSolution != null)
    {
        string X = GetResultX(optimalSolution);
        Console.WriteLine("\nОптимальний розв\'язок:");
        Console.WriteLine(X);
    }
}

while (true)
{
    Console.WriteLine("\nОберiть дiю:");
    Console.WriteLine("1 - Пошук оберненої матрицi");
    Console.WriteLine("2 - Пошук розв\'язку системи рiвнянь");
    Console.WriteLine("3 - Пошук рангу матрицi");
    Console.WriteLine("4 - Пошук опорного розв'зку");
    Console.WriteLine("5 - Пошук оптимального розв'зку");
    Console.WriteLine("6 - Видалення нульових рядкiв");
    Console.WriteLine("0 - Вихiд");
    Console.Write("Ваш вибiр: ");
    try
    {
        choice = int.Parse(Console.ReadLine());
    }
    catch (FormatException)
    {
        Console.WriteLine("Помилка! Введено некоректне число. Спробуйте ще раз.");
        continue;
    }

    switch (choice)
    {
        case 0:
            return;
        case 1:
            ReverseMatrix();
            break;
        case 2:
            SystemSolution();
            break;
        case 3:
            RankMatrix();
            break;
        case 4:
            ShowReferenceSolution();
            break;
        case 5:
            ShowOptimalSolution();
            break;
        case 6:
            DeleteZeroRows();
            break;
        default:
            Console.WriteLine("Помилка! Введено некоректний вибiр. Спробуйте ще раз.");
            continue;
    }
}