using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Twitterizer;
using System.IO;
using RcLibCs;

namespace Alicium2
{
	public class TwitterDo
	{
		private readonly Main m;
		public TwitterDo(Main _m)
		{
			m = _m;
		}
		public void Post(string Tweet)
		{
            new Task(() =>
            {
                try
                {
                    foreach (ExtendedOAuthTokens o in m.NowTokens)
                    {
                        var t = TwitterStatus.Update(o.OAuthTokens, Tweet);
                        if (t.Result != RequestResult.Success)
                        {
                            if (t.Result == RequestResult.RateLimited)
                            {
                                MConsole.WriteLine(o.UserName + " has been RateLimited. ResetDate must be " + t.RateLimiting.ResetDate.ToString() + ".");
                            }
                            else
                            {
                                MConsole.WriteLine("Some error happened : " + t.ErrorMessage);
                            }
                        }
                        else MConsole.WriteLine("Success to tweet with " + o.UserName + ": " + Tweet);
                    }
                }
                catch (Exception e)
                {
                    MConsole.WriteLine("Some error happened : " + e.Message);
                }
            }).Start();
		}
        public void Post(string Tweet, TwitterStatus ReplyTo)
		{
            new Task(() =>
            {
                try
                {
                    foreach (ExtendedOAuthTokens o in m.NowTokens)
                    {
                        var t = TwitterStatus.Update(o.OAuthTokens, Tweet, new StatusUpdateOptions() { InReplyToStatusId = ReplyTo.Id });
                        if (t.Result != RequestResult.Success)
                        {
                            if (t.Result == RequestResult.RateLimited)
                            {
                                MConsole.WriteLine(o.UserName + " has been RateLimited. ResetDate must be " + t.RateLimiting.ResetDate.ToString() + ".");
                            }
                            else
                            {
                                MConsole.WriteLine("Some error happened : " + t.ErrorMessage);
                            }
                        }
                        else MConsole.WriteLine("Success to tweet as " + o.UserName + " " + Tweet);
                    }
                }
                catch (Exception e)
                {
                    MConsole.WriteLine("Some error happened : " + e.Message);
                }
            }).Start();
		}
        public void MultiPoster(string[] Tweets)
        {
            new Task(() =>
                {
                    try
                    {
                        foreach (ExtendedOAuthTokens o in m.NowTokens)
                        {
                            foreach (string p in Tweets)
                            {
                                Post(p);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MConsole.WriteLine("Some error happened : " + e.Message);
                    }
                }).Start();
        }
        public void Favorite(TwitterStatus t)
		{
			new Task(() =>
	        {
                try
                {
                    foreach (ExtendedOAuthTokens o in m.NowTokens)
                    {
                        var f = TwitterFavorite.Create(o.OAuthTokens, t.Id);

                        if (f.Result != RequestResult.Success)
                        {
                            if (f.Result == RequestResult.RateLimited)
                            {
                                MConsole.WriteLine(o.UserName + " has been RateLimited. ResetDate must be " + f.RateLimiting.ResetDate.ToString() + ".");
                            }
                            else
                            {
                                MConsole.WriteLine("Some error happened : " + f.ErrorMessage);
                            }
                        }
                        else MConsole.WriteLine("Success to favorite [" + t.User.ScreenName + ": " + t.Text + "] as " + o.UserName);

                    }
                }
                catch(Exception e)
                {
                    MConsole.WriteLine("Some error happened : " + e.Message);
                }
	        }).Start();
		}
        public void Retweet(TwitterStatus t)
        {
			new Task(() =>
            {
                try
                {
                    foreach (ExtendedOAuthTokens o in m.NowTokens)
                    {
                        var f = TwitterStatus.Retweet(o.OAuthTokens, t.Id);
                        if (f.Result != RequestResult.Success)
                        {
                            if (f.Result == RequestResult.RateLimited)
                            {
                                MConsole.WriteLine(o.UserName + " has been RateLimited. ResetDate must be " + f.RateLimiting.ResetDate.ToString() + ".");
                            }
                            else
                            {
                                MConsole.WriteLine("Some error happened : " + f.ErrorMessage);
                            }
                        }
                        else MConsole.WriteLine("Success to retweet [" + t.User.ScreenName + ": " + t.Text + "] as " + o.UserName);
                    }
                }
                catch (Exception e)
                {
                    MConsole.WriteLine("Some error happened : " + e.Message);
                }
			}).Start();
		}
	}
	public class ExtendedOAuthTokens
	{
		public ExtendedOAuthTokens()
		{
			Tokens.Add(this);
		}
		OAuthTokens o;

		public static string Consumer_key = "RMQYKqZrL8JgpwH4h0Ypw";
		public static string Consumer_secret = "2yM7fktBX6SdeGdvedJ9Vj6Sti1tcRCC4sUSQXCxS4";
		public string AccessToken = "", AccessTokenSecret = "";
		public string UserName = "";
		public static List<ExtendedOAuthTokens> Tokens = new List<ExtendedOAuthTokens>();
		public ExtendedOAuthTokens Create(string _AccessToken, string _AccessSecret , string _UserName)
		{
			o = new OAuthTokens();
			o.AccessToken = AccessToken = _AccessToken;
			o.AccessTokenSecret = AccessTokenSecret = _AccessSecret;
			o.ConsumerKey = Consumer_key;
			o.ConsumerSecret = Consumer_secret;
			UserName = _UserName;
			return this;
		}
		public OAuthTokens OAuthTokens
		{
			get
			{
				return o;
			}
		}
	}

	public static class AccountReader
	{

		public static Dictionary<string, ExtendedOAuthTokens> Read(string path)
		{
            if (!File.Exists(path))
            {
                new FileInfo(path).Directory.Create();
                File.Create(path).Dispose();
            }
		    var d = new Dictionary<string, ExtendedOAuthTokens>();
			var read = Rc.CutString(";", Rc.loadData(path));
			foreach (var s in read)
			{
				var add = new ExtendedOAuthTokens().Create(Rc.CutString(",", s)[1], Rc.CutString(",", s)[2], Rc.CutString(",", s)[0]);
				d[Rc.CutString(",", s)[0]] = add;
			}
			return d;
		}

		public static bool Save(Dictionary<string, ExtendedOAuthTokens> d, string path)
		{
			try
			{
				string save = "";
				foreach (ExtendedOAuthTokens o in d.Values)
				{
					save += o.UserName + "," + o.AccessToken + "," + o.AccessTokenSecret + ";";
				}
				Rc.saveData(save, path);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
	public static class ColumnReader
	{
		public static Column[] Load(Main main)
		{
			int load = 0;
			var l = new List<Column>();
			while (File.Exists("Settings/Columns_" + load + ".dat"))
			{
				var data = (ColumnData)Rc.LoadFromXML<ColumnData>("Settings/Columns_" + load + ".dat");
				var add = data.ToColumn(main);
				if(add != null)
				{
					l.Add(add);
				}
				load++;
			}
			return l.ToArray();
		}
		public static bool Save(Column[] f)
		{
			//try
			{
				int load = 0;
				while (File.Exists("Settings/Columns_" + load + ".dat"))
				{
					File.Delete("Settings/Columns_" + load + ".dat");
					load++;
				}
				for (int i = 0; i < f.Length; i++)
				{
					Rc.SaveToXML<ColumnData>(f[i].ToColumnData(), "Settings/Columns_" + i + ".dat");
				}
                MessageBox.Show("Success to save.");
				return true;
			}
			/*catch
            {
                MessageBox.Show("Failed to save.");
                return false;
            }*/
		}
	}
    public static class Mariari
    {
        public static string[] Yakkai = new[]
        {
				"マリアリマリアリマリアリいいいいぃぃぃぃ！！！！１１１", 
				"ぃぃぃぃやあああああああああああああ！！！！", 
				"マリアリマリアリマリアリマリアリマリアリマリアリマリアリ！", 
				"魔理沙ちゃんとアリスちゃんの髪の毛クンカクンカくんかぁぁぁぁぁ", 
				"二人の御御姿を思いっきり覗きたい！！！！！！", 
				"魔理沙ちゃんとアリスちゃんの間に挟まれて死にたいよおおおおおお！！！！！！！！！！！！！！！！", 
				"魔理沙ちゃんとアリスちゃんの間に入って思いっきり深呼吸したい！！！！！！！！！", 
				"ﾋﾟｬｧｧｧｧｧｧｧｧｧｧｗｗｗｗｗｗ魔理沙ちゃんアリスちゃんラブラブらぶらぶ！！！！", 
				"二人の愛mogmogｗｗｗｗｗｗ二人の愛うめぇｗｗｗｗｗｗｗｗｗｗｗ", 
				"二人の汗飲みたいです＾＾", 
                "諸君　私はマリアリが好きだ",
                "諸君　私はマリアリが大好きだ",
                "百合百合が好きだ　アリス受けが好きだ　魔理沙受けが好きだ",
                "魔理沙の家で　アリスの家で　リビングで　キッチンで　お風呂で　ベッドで",
                "この地上で行われるありとあらゆるマリアリカップリングが大好きだ",
                "アリスの帰りを待ちドアの前に立ち尽くす魔理沙が好きだ",
                "ドアが開いた瞬間に魔理沙がアリスに抱きついた時など心がおどる",
                "ふたりが夕飯を洋食か和食かのどっちにするかで喧嘩をするのが好きだ",
                "結局二人で仲良く作ることになった時など胸がすくような気持ちだった",
                "二人が一緒の椅子に座ってご飯を食べるのが好きだ",
                "おたがいに相手のおかずが欲しくなってしまい口移しをしている様など感動すら覚える",
                "お風呂に入り背中を洗いあう様などはもうたまらない",
                "アリスの胸にばっかり視線が行ってしまう魔理沙も最高だ",
                "諸君　私はマリアリを百合畑の様なマリアリを望んでいる",
                "諸君　私に付き従う大隊戦友諸君",
                "君達は一体何を望んでいる？",
                "更なる百合百合を望むか？",
                "情け容赦のないシリアスなマリアリを望むか？",
                "百合百合の限りを尽くし新聞記者の鴉を萌え殺す嵐の様なマリアリを望むか？",
                "『マリアリ！　マリアリ！　マリアリ！』",
                "よ　ろ　し　い　　　な　ら　ば　マ　リ　ア　リ　だ",
                "第二次マリアリラブラブ作戦、状況を開始せよ　征くぞ、諸君",
                "このさきには牧歌的で百合のような萌殺マリアリどもがあなたをまっています",
                "そ　れ　で　も　同人誌を購入しますか？",
                "果たしてここまできたか。腹立たしいまでに厄介である。だがもっとも百合百合しい形に進んで来ているのはとても愉快だ。",
                "我がマリアリ素敵計画は君らの強い百合愛を以ってついに完遂されることとなる",
                "いよいよもって萌えるがよい。そしてさようなら。",
                "ｳｯﾋｮｫｫｫﾃﾛﾃﾛｸﾁｭｸﾁｭﾜｧｧｧｧﾌｫｵｵｵｵｵｵｵｵｵｵｵｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗｗ",
                "ﾋﾟｬｧｧｧｧｧｧ魔理沙とアリスのちゅっちゅちゅっちゅでしかももみもみきゃあああああｧｧｧｧｧｧｧｧｧｗｗｗｗｗｗｗｗｗｗｗｗ",
                "マリアリこそ正義ッ！！！！最強ッ！！！！そして至高おおおおおぉぉぉぉぉうっひゃああああああああァァァ！！！！！！ｗｗｗｗｗｗｗｗｗｗ",
                "同人誌のアリスちゃんが魔理沙ちゃんと仲良くしてるぞ！！！よかった…世の中まだまだ捨てたモンじゃないんだねっ！ ",
				"あれ…でも魔理沙ちゃんもアリスちゃんも現実世界にいない…？", 
				"いや！むしろ現実世界にいると僕が魔理沙ちゃんとアリスちゃんを覗き見できないぞ！", 
				"TLのマリアリジャスティスとかいう悪魔どもめ！！", 
                "二人とも僕の嫁だぞ！",
				"二人をくっつけるのは僕が許さないぞ！",
				"あれ…でも愛し合ってる二人もかわいい…？", 
				"なんてことだ！僕はいままでマリアリの本当の可愛さを知らなかったのか！待ってろよ二人とも！いま二人を結婚させにいくからね！！",
                "マリアリマリアリマリアリﾏﾘｱﾘﾏﾘｱﾘﾏﾘｱﾘまいりあいmりありrまりいまっりみいあmりらmmmmmmmmmmmmmmmm",
                "私は超越しました。 http://cannotdebug.blog.fc2.com/blog-entry-1.html #Mariari_Tairiku"
        };
        public static string[] Cannon = new string[]
        {
            "お菓子食べてる　アリス「あら、最後の一個ね」魔理沙「私が食べるぜ！！」ぱくっ　アリス「え、ちょっ」魔理沙「もぐもぐ」アリス「むぅ・・・」魔理沙「えいっ」ちゅー　アリス「！？！？///（あ、甘い・・・！）」魔理沙「ぷはぁ・・・口移しだぜ☆」アリス「も、もう///」",
            "人形遊び中　アリス「アリス！ダイスキダゼ！」アリス「そ、そう？私も大好きよ・・・///」アリス「・・・ソノ、キスシヨウゼ」アリス「ま、まりさっ・・・///」魔理沙「・・・アリス、何やってるんだぜ」アリス「！！！！！！！！！！！！」",
            "アリス「む、奴の気配」パリーン！　魔理沙「お邪魔するぜー！」アリス「・・・窓から入るなって言ってるじゃない」魔理沙「あ・・・ごめん、つい」アリス「つい、ってなによ」魔理沙「ホントごめん、以後気をつけるぜ」アリス「気をつけなさいよ」",
            "魔理沙「アリスぅ～」アリス「なによ」魔理沙「膝枕してだぜ」アリス「もう、しょうがないわね・・・」ぺたん　魔理沙「うー、やわらかくて寝心地最高だぜ」アリス「お、重い・・・ちょ、魔理沙」魔理沙「Zzz......」アリス「・・・くすっ」なでなで",
            "魔理沙「おなか空いたぜ・・・」アリス「そうね、ご飯にしまｓ」魔理沙「わ、私が作るぜ！」アリス「・・・料理できるの？」魔理沙「も、もちろんだぜ！」30分後　魔理沙「できたぜ！（どやっ」アリス「（お、おいしい！！！！！！見た目はアレだけど）」",
            "アリス「♪～」魔理沙「何編んでるんだぜ」アリス「マフラーよ」魔理沙「ふーん」アリス「♪～～」魔理沙「・・・私のも作ってくれよ」アリス「あんたのために編んでるのよ？」魔理沙「！！///」アリス「はい、できたわよ」魔理沙「あ、ありがとだぜ///」",
            "ベッドで　魔理沙「・・・もう寝ちゃうんだぜ？」アリス「え？」魔理沙「アリスともっとくっつきたいぜ」アリス「もう・・・ちょっとだけよ？」ぎゅっ　魔理沙「あったかいぜ」アリス「そう？」魔理沙「・・・アリスーっ！！」ぎゅーっ　アリス「も、もう寝るんだってば///」",
            "お風呂で　魔理沙「アリス・・・」アリス「なに？」魔理沙「お前・・・胸意外と大きいんだな」アリス「え、えっ？」魔理沙「仲間だとおもってたぜ・・・orz」アリス「・・・個人差よ、あきらめなさい・・・」魔理沙「悔しいぜ」もみもみ　アリス「や、やめなさいっ///」",
            "戦闘中　魔理沙「アリス、アレをやるぞ！！」アリス「う、うん！」手ぎゅっ　魔理沙「私のマスタースパークと！」アリス「私の魔彩光で！」",
            "魔理沙＆アリス「マリス砲ーーーーーーーーーーーーーーーーーーーー！！！！！！」",
            "ピチューン http://cannotdebug.blog.fc2.com/blog-entry-1.html #Mariari_Tairiku"
        };
        public static string Stealth = "ショートカットキー至上クライアント「Alicium 2.x」を使っています。 http://cannotdebug.blog.fc2.com/blog-entry-9.html #Alicium_2";
    }
	public class ColumnData
	{
		public string Track;
		public string Follow;
		public string AccountName;
		public string Tille;
        public string Image;
		public Column.StartType ColumnType;
		public ColumnData Set(string _AccountName, string _Title, string _Track, string _Follow, Column.StartType Start)
		{
			return new ColumnData() { AccountName = _AccountName, Tille = _Title, Track = _Track, Follow = _Follow, ColumnType = Start,Image = "null"};
		}
		public ColumnData Set(string _AccountName, string _Title, string _Track, string _Follow, Column.StartType Start,string b)
		{
			return new ColumnData() { AccountName = _AccountName, Tille = _Title, Track = _Track, Follow = _Follow, ColumnType = Start,Image = b};
		}
		public Column ToColumn(Main m)
		{
			var v = new Twitterizer.Streaming.StreamOptions();
			v.Track.AddRange(Rc.CutString(",", Track));
			v.Follow.AddRange(Rc.CutString(",", Follow));
			try
			{
                if (Image == "null")
                    return new Column(new Twitterizer.Streaming.TwitterStream(m.Accounts[AccountName].OAuthTokens, "Alicium", v), ColumnType, Tille);
                else
                {
                    using (var f = File.OpenRead(Image))
                    {
                        var b = Bitmap.FromStream(f);
                        return new Column(new Twitterizer.Streaming.TwitterStream(m.Accounts[AccountName].OAuthTokens, "Alicium", v), ColumnType, Tille, (Image)b.Clone());
                    }
                }
			}
			catch
			{
				MessageBox.Show("Can't find "+AccountName+" from accounts data.Please authenticate again.","Not Found",MessageBoxButtons.OK,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1,MessageBoxOptions.DefaultDesktopOnly,false);
				return null;
			}
		}
	}
}