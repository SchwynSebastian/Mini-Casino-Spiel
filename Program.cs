using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
       
        CasinoGame casinoGame = new CasinoGame();
        PlayerObserver playerObserver = new PlayerObserver();

        
        casinoGame.Attach(playerObserver);

        
        casinoGame.Start();
    }
}

public interface IObserver
{
    void Update(string message);
}

public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(string message);
}

public class CasinoGame : ISubject
{
    private List<IObserver> observers = new List<IObserver>();
    private BalanceManager balanceManager = BalanceManager.Instance;

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify(string message)
    {
        foreach (var observer in observers)
        {
            observer.Update(message);
        }
    }

    public void Start()
    {
        while (true)
        {
            Console.Clear();
            ShowMainMenu();

            string choice = Console.ReadLine();
            if (choice == "1")
            {
                PlayRoulette();
            }
            else if (choice == "2")
            {
                PlayBlackjack();
            }
            else if (choice == "3")
            {
                break;
            }
        }
    }

    public void ShowMainMenu()
    {
        Console.WriteLine("Willkommen im Casino!");
        Console.WriteLine("1. Roulette spielen");
        Console.WriteLine("2. Blackjack spielen");
        Console.WriteLine("3. Beenden");
        Console.Write("Wähle eine Option: ");
    }

    public int GetBetAmount()
    {
        while (true)
        {
            Console.Write("Setze deinen Einsatz: ");
            if (int.TryParse(Console.ReadLine(), out int bet) && bet > 0 && bet <= balanceManager.Balance)
            {
                return bet;
            }
            Console.WriteLine("Ungültiger Einsatz. Versuche es erneut.");
        }
    }

    // Roulette spielen
    public void PlayRoulette()
    {
        Console.Clear();
        Console.WriteLine("Roulette Spiel");
        Console.WriteLine($"Dein Guthaben: {balanceManager.Balance} Chips");

        int betAmount = GetBetAmount();
        string betType = GetBetType();

        int winningNumber = new Random().Next(0, 37);
        Console.WriteLine($"Die Kugel fällt auf: {winningNumber}");

        if (IsWinningRouletteBet(betType, winningNumber))
        {
            int winnings = CalculateRouletteWinnings(betType, betAmount);
            Console.WriteLine($"Du hast gewonnen! Gewinn: {winnings} Chips");
            balanceManager.Balance += winnings; 
            Notify($"Neues Guthaben: {balanceManager.Balance} Chips");
        }
        else
        {
            Console.WriteLine("Leider verloren.");
            balanceManager.Balance -= betAmount; 
            Notify($"Neues Guthaben: {balanceManager.Balance} Chips");
        }

        Console.WriteLine("Drücke Enter für eine neue Runde oder 'q' für das Menü.");
        string input = Console.ReadLine().ToLower();
        if (input == "q")
        {
            return; 
        }
        else
        {
            PlayRoulette();
        }
    }

    public string GetBetType()
    {
        while (true)
        {
            Console.Write("Setze auf 'rot', 'schwarz', '1-12', '13-24', '25-36', 'gerade', 'ungerade', '1-18', '19-36' oder eine Zahl (0-36): ");
            string bet = Console.ReadLine().ToLower();
            if (IsValidRouletteBet(bet))
            {
                return bet;
            }
            Console.WriteLine("Ungültige Wette. Versuche es erneut.");
        }
    }

    public bool IsValidRouletteBet(string bet)
    {
        return bet == "rot" || bet == "schwarz" || bet == "1-12" || bet == "13-24" || bet == "25-36"
            || bet == "gerade" || bet == "ungerade" || bet == "1-18" || bet == "19-36" || int.TryParse(bet, out _);
    }

    public bool IsWinningRouletteBet(string bet, int result)
    {
        if (bet == "rot" && IsRed(result)) return true;
        if (bet == "schwarz" && !IsRed(result) && result != 0) return true;
        if (bet == "1-12" && result >= 1 && result <= 12) return true;
        if (bet == "13-24" && result >= 13 && result <= 24) return true;
        if (bet == "25-36" && result >= 25 && result <= 36) return true;
        if (bet == "gerade" && result % 2 == 0 && result != 0) return true;
        if (bet == "ungerade" && result % 2 == 1) return true;
        if (bet == "1-18" && result >= 1 && result <= 18) return true;
        if (bet == "19-36" && result >= 19 && result <= 36) return true;
        if (int.TryParse(bet, out int number) && number == result) return true;
        return false;
    }

    public int CalculateRouletteWinnings(string bet, int betAmount)
    {
        if (int.TryParse(bet, out int num) && num == 0) return betAmount * 36;
        if (int.TryParse(bet, out _)) return betAmount * 35;
        if (bet == "1-12" || bet == "13-24" || bet == "25-36") return betAmount * 3;
        return betAmount * 2;
    }

    public bool IsRed(int number)
    {
        int[] redNumbers = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
        return Array.Exists(redNumbers, n => n == number);
    }

    // Blackjack spielen
    public void PlayBlackjack()
    {
        Console.Clear();
        Console.WriteLine("Blackjack Spiel");
        Console.WriteLine($"Dein Guthaben: {balanceManager.Balance} Chips");

        int betAmount = GetBetAmount();
        List<int> playerHand = new List<int> { DrawCard(), DrawCard() };
        List<int> dealerHand = new List<int> { DrawCard() };

        Console.WriteLine($"Deine Karten: {string.Join(", ", playerHand)} (Wert: {GetHandValue(playerHand)})");
        Console.WriteLine($"Dealer zeigt: {dealerHand[0]}");

        while (GetHandValue(playerHand) < 21)
        {
            Console.Write("Hit (h) oder Stand (s)? ");
            string choice = Console.ReadLine().ToLower();

            if (choice == "h")
            {
                playerHand.Add(DrawCard());
                Console.WriteLine($"Neue Karten: {string.Join(", ", playerHand)} (Wert: {GetHandValue(playerHand)})");
            }
            else if (choice == "s")
            {
                break;
            }
        }

        int playerValue = GetHandValue(playerHand);
        if (playerValue > 21)
        {
            Console.WriteLine("Du hast überzogen! Verloren.");
            balanceManager.Balance -= betAmount;
            Notify($"Neues Guthaben: {balanceManager.Balance} Chips");
        }
        else
        {
            dealerHand.Add(DrawCard());
            DealerPlay(ref dealerHand);
            int dealerValue = GetHandValue(dealerHand);

            Console.WriteLine($"Dealer hat: {string.Join(", ", dealerHand)} (Wert: {dealerValue})");

            if (dealerValue > 21 || playerValue > dealerValue)
            {
                Console.WriteLine("Du hast gewonnen!");
                balanceManager.Balance += betAmount;
                Notify($"Neues Guthaben: {balanceManager.Balance} Chips");
            }
            else if (playerValue < dealerValue)
            {
                Console.WriteLine("Der Dealer gewinnt.");
                balanceManager.Balance -= betAmount;
                Notify($"Neues Guthaben: {balanceManager.Balance} Chips");
            }
            else
            {
                Console.WriteLine("Unentschieden!");
            }
        }

        Console.WriteLine("Drücke Enter für eine neue Runde oder 'q' für das Menü.");
        string input = Console.ReadLine().ToLower();
        if (input == "q")
        {
            return; 
        }
        else
        {
            PlayBlackjack(); 
        }
    }

    public void DealerPlay(ref List<int> dealerHand)
    {
        while (GetHandValue(dealerHand) < 17)
        {
            dealerHand.Add(DrawCard());
        }
    }

    public int DrawCard()
    {
        return new Random().Next(1, 11);
    }

    public int GetHandValue(List<int> hand)
    {
        int total = 0;
        foreach (int card in hand) total += card;
        return total;
    }
}

public class PlayerObserver : IObserver
{
    public void Update(string message)
    {
        Console.WriteLine($"[Observer] {message}");
    }
}

public class BalanceManager
{
    private static BalanceManager _instance;
    private int balance;
    public const int InitialBalance = 1000;

    private BalanceManager()
    {
        balance = InitialBalance;
    }

    public static BalanceManager Instance => _instance ??= new BalanceManager();

    public int Balance
    {
        get => balance;
        set => balance = value;
    }
}
