using CardGameCorner.Models;

public class GameDetailsResponse
{
    public Data Data { get; set; }

    public List<Banner> Banners { get; set; }

}

public class Banner
{
    public string Game { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public string Url { get; set; }
}


public class Data
{
    public List<Result> Results { get; set; }
   
}



public class Result
{
    public Image image { get; set; }
    public Model model { get; set; }
    public Novita novita { get; set; }
    public Maxprice maxprice { get; set; }
    public Minprice minprice { get; set; }

    public Url url { get; set; }
    public Url urlen { get; set; }
}


public class Url 
{
    public string raw { get; set; }
}

public class Image
{
    public string raw { get; set; }
}

public class Model
{
    public string snippet { get; set; }
}

public class Novita
{
    public string raw { get; set; }
}

public class Maxprice
{
    public decimal raw { get; set; }
}

public class Minprice
{
    public decimal raw { get; set; }
}
