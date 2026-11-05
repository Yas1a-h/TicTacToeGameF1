using System;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToeGameF1
{
    public partial class Form1 : Form
    {
        private const int Rows = 12;
        private const int Columns = 12;

        private readonly Button[,] cells = new Button[Rows, Columns];
        private readonly Random random = new Random();
        private readonly Color[] palette =
        {
            Color.LightCoral, Color.LightSkyBlue, Color.LightGreen,
            Color.Khaki, Color.Plum, Color.PeachPuff,
            Color.LightSalmon, Color.PaleTurquoise, Color.Thistle,
            Color.LightSteelBlue, Color.PaleGreen, Color.MistyRose
        };

        private int player1Score;
        private int player2Score;
        private char currentPlayer = 'X';
        private bool gameRunning;

        private Label player1Label;
        private Label player2Label;
        private Label turnLabel;
        private Label statusLabel;
        private TableLayoutPanel boardPanel;

        public Form1()
        {
            InitializeComponent();
            BuildInterface();
            StartNewGame();
        }

        private void BuildInterface()
        {
            Text = "TicTacToeGameF1";
            MinimumSize = new Size(850, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(35, 38, 45);

            // Sektor A ma 80% szerokoœci, a sektor B 20%.
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = BackColor,
                Padding = new Padding(8)
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Controls.Clear();
            Controls.Add(mainLayout);

            var sectorA = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
            var sectorB = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = Color.FromArgb(48, 52, 60),
                BorderStyle = BorderStyle.FixedSingle
            };

            mainLayout.Controls.Add(sectorA, 0, 0);
            mainLayout.Controls.Add(sectorB, 1, 0);

            boardPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = Rows,
                ColumnCount = Columns,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                BackColor = Color.FromArgb(25, 27, 32),
                Padding = new Padding(2)
            };

            for (int c = 0; c < Columns; c++)
                boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / Columns));

            for (int r = 0; r < Rows; r++)
                boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / Rows));

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    var cell = new Button
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(1),
                        Text = "",
                        Font = new Font("Arial", 18F, FontStyle.Bold),
                        FlatStyle = FlatStyle.Flat,
                        UseVisualStyleBackColor = false,
                        Tag = new Point(r, c),
                        BackColor = palette[random.Next(palette.Length)]
                    };

                    cell.FlatAppearance.BorderSize = 1;
                    cell.FlatAppearance.BorderColor = Color.FromArgb(70, 70, 70);
                    cell.Click += Cell_Click;
                    cells[r, c] = cell;
                    boardPanel.Controls.Add(cell, c, r);
                }
            }

            sectorA.Controls.Add(boardPanel);

            var sideLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 6,
                BackColor = Color.Transparent
            };

            var heading = new Label
            {
                Text = "TIC TAC TOE",
                Dock = DockStyle.Fill,
                Height = 55,
                ForeColor = Color.White,
                Font = new Font("Arial", 16F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            player1Label = CreateInfoLabel();
            player2Label = CreateInfoLabel();
            turnLabel = CreateInfoLabel();
            statusLabel = CreateInfoLabel();

            var startButton = CreateControlButton("Start");
            var stopButton = CreateControlButton("Stop");
            var restartButton = CreateControlButton("Restart");

            startButton.Click += (s, e) => StartNewGame();
            stopButton.Click += (s, e) => StopGame();
            restartButton.Click += (s, e) => RestartBoard();

            sideLayout.Controls.Add(heading, 0, 0);
            sideLayout.Controls.Add(player1Label, 0, 1);
            sideLayout.Controls.Add(player2Label, 0, 2);
            sideLayout.Controls.Add(turnLabel, 0, 3);
            sideLayout.Controls.Add(statusLabel, 0, 4);

            var buttonsLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true
            };
            buttonsLayout.Controls.Add(startButton);
            buttonsLayout.Controls.Add(stopButton);
            buttonsLayout.Controls.Add(restartButton);
            sideLayout.Controls.Add(buttonsLayout, 0, 5);

            sectorB.Controls.Add(sideLayout);
        }

        private Label CreateInfoLabel()
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                MinimumSize = new Size(0, 48),
                ForeColor = Color.White,
                Font = new Font("Arial", 16F),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(4)
            };
        }

        private Button CreateControlButton(string text)
        {
            return new Button
            {
                Text = text,
                Width = 150,
                Height = 48,
                Margin = new Padding(4, 8, 4, 8),
                Font = new Font("Arial", 16F, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(65, 70, 80),
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = false
            };
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            if (!gameRunning || !(sender is Button clickedCell))
                return;

            Point position = (Point)clickedCell.Tag;
            int row = position.X;
            int column = position.Y;

            if (!string.IsNullOrEmpty(clickedCell.Text))
                return;

            clickedCell.Text = currentPlayer.ToString();
            clickedCell.ForeColor = currentPlayer == 'X' ? Color.Green : Color.Blue;
            clickedCell.Font = new Font("Arial", 18F, FontStyle.Bold);
            clickedCell.Enabled = false;

            int points = CountLinesThroughCell(row, column, currentPlayer);
            if (currentPlayer == 'X')
                player1Score += points;
            else
                player2Score += points;

            // Po ka¿dym ruchu losujemy ponownie kolory wszystkich pól.
            RandomizeBoardColors();

            int freeFields = CountFreeFields();
            if (freeFields < 5)
            {
                gameRunning = false;
                statusLabel.Text = "Koniec gry!";
                turnLabel.Text = "Gra zakoñczona";
                SetBoardEnabled(false);
            }
            else
            {
                currentPlayer = currentPlayer == 'X' ? 'O' : 'X';
                statusLabel.Text = "Wolne pola: " + freeFields;
                turnLabel.Text = "Ruch gracza: " + currentPlayer;
            }

            UpdateLabels();
        }

        // Liczy wszystkie uk³ady piêciu pól zawieraj¹ce ostatnio wykonany ruch.
        private int CountLinesThroughCell(int row, int column, char symbol)
        {
            int[,] directions =
            {
                { 0, 1 }, { 1, 0 }, { 1, 1 }, { 1, -1 }
            };

            int total = 0;

            for (int d = 0; d < directions.GetLength(0); d++)
            {
                int rowStep = directions[d, 0];
                int columnStep = directions[d, 1];

                for (int offset = 0; offset < 5; offset++)
                {
                    int startRow = row - offset * rowStep;
                    int startColumn = column - offset * columnStep;
                    bool completeLine = true;

                    for (int i = 0; i < 5; i++)
                    {
                        int checkRow = startRow + i * rowStep;
                        int checkColumn = startColumn + i * columnStep;

                        if (checkRow < 0 || checkRow >= Rows ||
                            checkColumn < 0 || checkColumn >= Columns ||
                            cells[checkRow, checkColumn].Text != symbol.ToString())
                        {
                            completeLine = false;
                            break;
                        }
                    }

                    if (completeLine)
                        total++;
                }
            }

            return total;
        }

        private void RandomizeBoardColors()
        {
            foreach (Button cell in cells)
                cell.BackColor = palette[random.Next(palette.Length)];
        }

        private int CountFreeFields()
        {
            int free = 0;
            foreach (Button cell in cells)
            {
                if (string.IsNullOrEmpty(cell.Text))
                    free++;
            }
            return free;
        }

        private void StartNewGame()
        {
            player1Score = 0;
            player2Score = 0;
            currentPlayer = 'X';
            gameRunning = true;
            ClearBoard();
            SetBoardEnabled(true);
            RandomizeBoardColors();
            statusLabel.Text = "Nowa gra";
            UpdateLabels();
        }

        // Restart czyœci planszê, ale zachowuje wyniki graczy.
        private void RestartBoard()
        {
            currentPlayer = 'X';
            gameRunning = true;
            ClearBoard();
            SetBoardEnabled(true);
            RandomizeBoardColors();
            statusLabel.Text = "Plansza zresetowana";
            UpdateLabels();
        }

        private void StopGame()
        {
            gameRunning = false;
            SetBoardEnabled(false);
            statusLabel.Text = "Gra zatrzymana";
            turnLabel.Text = "Gra zakoñczona";
        }

        private void ClearBoard()
        {
            foreach (Button cell in cells)
            {
                cell.Text = "";
                cell.Enabled = true;
                cell.ForeColor = Color.Black;
            }
        }

        private void SetBoardEnabled(bool enabled)
        {
            foreach (Button cell in cells)
                cell.Enabled = enabled && string.IsNullOrEmpty(cell.Text);
        }

        private void UpdateLabels()
        {
            player1Label.Text = "Gracz 1 (X): " + player1Score;
            player2Label.Text = "Gracz 2 (O): " + player2Score;
            if (gameRunning)
                turnLabel.Text = "Ruch gracza: " + currentPlayer;
        }
    }
}
