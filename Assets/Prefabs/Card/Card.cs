using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum Source
{
    DECK,
    COLUMN
}

public class Card : MonoBehaviour
{
    public DragController dragController;
    public string seed { get; private set; }
    public string num { get; private set; }
    public int intNum { 
        get { 
            return NumToString.getNum(num); 
        }
    }
    public Source source {
        get { return dragController.source; }
        set { dragController.source = source; } 
    }
    CardStruct selfStruct;
    public Color color
    {
        get
        {
            return Seed.getSeedColor(seed);
        }
    }

    [SerializeField] float textSize = 8;
    [SerializeField] TextMeshPro textNum;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] List<Sprite> spriteSeed;

    void Start()
    {
        textSize = textNum.fontSize;
    }
    public override string ToString()
    {
        return num + " di " + seed + "  (n: " + intNum + ")";
    }
    public CardStruct ToStruct()
    {
        return selfStruct;
    }

    void UpdateSprite()
    {
        textNum.text = num;
        textNum.color = Seed.getSeedColor(seed);
        textNum.fontSize = (num == "Q") ? textSize - 1 : textSize;
        spriteRenderer.sprite = spriteSeed[Seed.getSeedIndex(seed)];
    }
    private void Init(string s, string n, Source source)
    {
        num = n;
        seed = s;
        this.source = source;
        selfStruct = new CardStruct(s, n);
        UpdateSprite();
    }
    [ContextMenu("TestA")]
    void TestA()
    {
        Init(Seed.cuori, "A", Source.DECK);
    }

#region Public Init Funcs
    public void Init(string s, int n, Source source)
    {
        s = Seed.Validate(s) ? s : "Wrong";  // "Wrong" seed is handled by Seed getIndex and UpdateSprite -- it shows big red X
        Init(s, NumToString.getLetter(n), source);
    }

    [ContextMenu("Randomize")]
    public void Init()
    {
        Init(Seed.getRandomSeed(), NumToString.getRandomNum(), Source.DECK);
    }

    public void Init(Source s)
    {
        Init(); // still random 
        source = s; // apply source 
    }

    public void Init(Card c)
    {
        Init(c.seed, c.num, c.source);
    }
    public void Init(CardStruct c)
    {
        Init(c.seed, c.num, Source.DECK);
    }
#endregion
}

public struct CardStruct
{
    public string seed;
    public string num;

    public CardStruct(string s, string n)
    {
        seed = s;
        num = n;
    }
}

static class Seed
{
    public const string cuori = "Cuori";
    public const string denari = "Denari";
    public const string fiori = "Fiori";
    public const string picche = "Picche";
    static string[] seeds = new string[] { cuori, denari, fiori, picche };

    static Color redColor = new Color32(194, 25, 36, 255); // same red shade as the sprites
    static Color blackColor = Color.black;

    public static bool Validate(string s)
    {
        return seeds.Contains(s);
    }

    public static int getSeedIndex(string s)
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            if (seeds[i] == s) return i;
        }
        return seeds.Length;   // if wrong seed, then return 4 to display big red X
    }

    public static string getRandomSeed()
    {
        int rand = Random.Range(0, seeds.Length); // max value excluded
        return seeds[rand];
    }

    public static Color getSeedColor(string s)
    {
        if (getSeedIndex(s) < 2)    
            return redColor;        // 0,1 = cuori, denari
        else
            return blackColor;      // 2,3 = fiori, picche
    }
}

static class NumToString
{
    public const int minNum = 2;
    public const int maxNum = 14;
    static string[] letters = new string[] { "J", "Q", "K", "A" };

    public static string getLetter(int num)
    {
        if (num > maxNum || num < minNum)
        {
            if (num != 1) Debug.LogWarning("Number wrong!");  // 1 and A are the same card, so no error
            num = maxNum;
        }
        return (num > 10) ? letters[num - 11] : num.ToString();
    }
    public static int getNum(string num)
    {
        if (num == "A") return 1;   // A is the first card in solitaire order
        int n;
        bool canParse = int.TryParse(num, out n);
        if (canParse && n >= minNum && n <= 10)
            return n;
        else{
            for (int i = 0; i < letters.Length; i++)
            {
                if (letters[i] == num) return i+11;
            }
            Debug.LogError("Wrong number!");
            return 0;
        }
    }

    public static string getRandomNum()
    {
        int rand = Random.Range(minNum, maxNum + 1); // max value excluded
        return getLetter(rand);
    }
}