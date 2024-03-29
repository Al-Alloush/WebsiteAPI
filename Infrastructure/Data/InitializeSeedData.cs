﻿using Core.Models;
using Core.Models.Blogs;
using Core.Models.Identity;
using Core.Models.Uploads;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class InitializeSeedData
    {
        private static CultureInfo MyCultureInfo = new CultureInfo("de-DE");
        private static string[] DefLanguageCodeId = InitializeDefaultData.DefLanguageCodeId;

        private static string UploadBlogImageDir = "/Uploads/Blogs/Images/";
        public static string UploadUsersImageDir = "/Uploads/Users/Images/";

        public static async Task AddSeedUsersData(IServiceProvider services)
        {

            // Get UserManager Service to Application
            UserManager<AppUser> userManager = services.GetRequiredService<UserManager<AppUser>>();
            var context = services.GetRequiredService<AppDbContext>();
            Random random = new Random();

            // create Users list
            for (int i = 1; i <= 100; i++)
            {
                var Confirmed = true;
                var name = "visitor";
                var role = "Visitor";
                var AuPass = false;

                if (i >= 20 && i < 25)
                {
                    AuPass = true;
                }

                if (i >= 25 && i < 30)
                {
                    name = "admin";
                    role = "Admin";
                }
                if (i >= 30 && i < 35)
                {
                    name = "editor";
                    role = "Editor";
                }
                if (i >= 40 && i < 50)
                {
                    Confirmed = false;
                }

                var userS = new AppUser
                {
                    Email = name + "_" + i + "@al-alloush.com",
                    EmailConfirmed = Confirmed,
                    UserName = name + "_" + i,
                    FirstName = "First " + name + " Name",
                    LastName = "Last " + name + " Name",
                    RegisterDate = DateTime.Now,
                    Birthday = DateTime.Parse("15/6/1978", MyCultureInfo),
                    PhoneNumber = "17328547" + i,
                    PhoneNumberConfirmed = Confirmed,
                    AutomatedPassword = AuPass,
                    Address = new Address
                    {
                        Street = "street_" + i,
                        BuildingNum = "12",
                        Flore = "L 3",
                        City = "city_" + i,
                        Zipcode = "874" + i,
                        Country = "German"
                    }
                };
                IdentityResult createS = await userManager.CreateAsync(userS, "!QA1qa");
                if (createS.Succeeded)
                    userManager.AddToRoleAsync(userS, role).Wait();
            }

            var users = await context.Users.ToArrayAsync();
            for (int i = 1; i < users.Length; i++)
            {

                var imgNum = random.Next(1, 52);
                var userImage = new UploadUserImagesList
                {

                    Name = "user (" + imgNum + "): " + random.Next(1, 100000),
                    Path = UploadUsersImageDir + "user (" + imgNum + ").jpg",
                    UserId = users[i].Id,
                    Default = true,
                    UploadTypeId = 1
                };
                await context.UploadUserImagesList.AddAsync(userImage);
            }
            await context.SaveChangesAsync();

            // 
            for (int iu = 0; iu < users.Length; iu++)
            {
                string[] languages = { "de", "ar" };
                // add default languages
                for (int i = 0; i < 2; i++)
                {
                    var lang = new UserSelectedLanguages
                    {
                        UserId = users[iu].Id,
                        LanguageId = languages[i]
                    };

                    await context.UserSelectedLanguages.AddAsync(lang);
                }
                await context.SaveChangesAsync();
            }
        }

        private static string[] bodyEn =
        {
                    "A delicious and delicious dish with a beautiful shape that can be prepared from the vegetables that are always present in the Arab cuisine",
                     "Appetizers, appetizers, mezze (in the Levant), or Moftahat (in Tunisia) is a food item of vegetables or fruits that is cooked, pickled or minced, and it is usually used as a seasoning, especially to beautify the main dish.",
                     "Pickle is made from pickling (pickle it), that is, put it in a substance that has been mixed with it",

                    "Man experienced travel from the beginning of his existence on the planet, when he was obliged to move in search of water and food, then he continued with that after he became rich from the hardships of the road, as the human soul became unbearable.",
                    "The positive and negative aspects of travel and feelings are governed by the nature of a person on the one hand and the nature of his travel and his condition on the other hand.",

                    "Fairouz is a Lebanese singer. Her real name is'Nihad Rizk Wadih Haddad', she was born in November 1935 in a neighborhood called'Zuqaq El Blat'in Beirut. She performed many songs and operas.",
                    "Laila Murad is an Egyptian singer and actress. She is considered one of the most prominent singers and actresses in the Arab world in the twentieth century. She started her singing career at the age of fourteen years",
                    "Sayed Darwish is an Egyptian singer and composer, whose real name is Mr. Darwish Al-Bahr. He is the renewer of music and the catalyst for musical renaissance [?] In Egypt and the Arab world.",

                    "The Summer Olympics, or the Summer Olympics, is a sporting event that takes place every four years for the Olympic Games.",
                     "LeBron James finished first among the NBA players and generally second after Cristiano Ronaldo, the Real Madrid star who topped the list. Kevin Durant finished eighth.",

                    "He defined fitness by Clarke, 1976 as the ability to carry out daily tasks with strength, awareness, and without undue fatigue from the availability of sufficient energy.",
                    "They are the physical features that an individual has to achieve in order to do normal daily chores with the best efficiency without feeling tired and exhausted. Daily chores are simple, but they may become more complex to include competitive sports and more difficult activities.",
                    "Flexibility is the joint's ability to move to the farthest extent within its range of motion! Examples of exercises that improve flexibility are yoga exercises and stretch the body. For example, put your feet on the ground with a distance between them.",

                   "Mars or the red planet is the fourth planet in terms of distance from the sun in the solar system and it is the outermost neighbor of the earth and it is classified as a rocky planet.",
                    "The massacre of the castle, or the massacre of the Mamluks, is a historical incident that occurred in the Ottoman province of Egypt, which was orchestrated by Muhammad Ali Pasha to get rid of his Mamluk enemies on Friday 5 Safar 1226 AH corresponding to March 1.",
                    "The United States of America has detained more than 779 extrajudicial administrative detainees at the American Guantanamo Bay detention center in Cuba since the opening of the detention camps on January 11, 2002.",
                    "The cultural movement is every change in the way of thinking and prevailing beliefs regarding a group of cultural issues related to certain aspects and branches, which results in a change in the way we approach and treat work.",
                    "Applied engineering means applying science to meet human needs, through the application of theoretical and applied sciences: physics, chemistry, mathematics, and biology.",
                    "A robot or a robot (in English: Robot) is a mechanical tool capable of carrying out pre-programmed activities, and the robot performs these activities.",
                    "Imhotep was the builder of the stepped pyramid of Djoser, the first architect, and one of the most famous engineers in ancient Egypt. He was raised to the rank of an idol after his death and became the god of medicine."
                };

        private static string[] bodyAr =
        {
                    "طبق شهي و لذيذ شكله جميل يمكن تحضيره من الخضار المتواجدة دائما في المطبخ العربي",
                    " المقبلات أو المشهيات أو المزة (في الشام) أو المفتحات (في تونس) هي مادة غذائية من الخضار أو الفاكهة تكون مطبوخا أو مخللا أو مفرومًا وعادة ما يُستخدم كـتوابل خصوصًا لتجميل الطبق الرئيسي. ",
                    "المخلل من التخليل (خلّله) أي وضعه في مادة تخلّله فالخيار مثلاً ينقع في الماء والملح في إناء مضغوط ومسحوب منه الهواء لفترة أسبوع",

                    "ختبر الإنسانُ السَّفر منذ بداية وجوده على المعمورة، حيث كان مضطرًّا للتنقُّل بحثًا عن الماء والطَّعام، ثمَّ استمرَّ في ذلك بعد أنْ أصبح في غنيةٍ عن مشقَّة الطَّريق، فقد فطرتِ النَّفس البشريَّة",
                    "إيجابيات السَّفر وسلبياته والمشاعر يحكمها طبع الإنسان من جهةٍ وطبيعة سفره وحاله من جهةٍ أخرى؛ فمن خبر محاسن الغربة ووجد فيها ملاذًا مطمئنًا له",

                    "فيروز مغنية لبنانية. اسمها الحقيقي «نهاد رزق وديع حداد»، ولدت في نوفمبر من سنة 1935 في حي يسمى 《زقاق البلاط》 في بيروت، قدمت العديد من الأغاني والأوبريهات",
                    "ليلى مراد مغنية وممثلة مصرية، تعتبر من أبرز المغنيات والممثلات في الوطن العربي في القرن العشرين. بدأت مشوارها مع الغناء في سن أربعة عشر عامًا",
                    "سيد درويش مغني وملحن مصري اسمه الحقيقي السيد درويش البحر هو مجدد الموسيقى وباعث النهضة[؟] الموسيقية في مصر والوطن العربي. بدأ ينشد مع أصدقائه ألحان الشيخ",

                    "لألعاب الأولمبية الصيفية ، أو ألعاب الأولمبياد الصيفية ، هي حدث رياضي يقام كل أربع سنوات لممارسة الألعاب الأولمبية",
                    " ليبرون جيمس حل أولاً بين لاعبي NBA وفي المركز الثاني عامة بعد كريستيانو رونالدو نجم الريال الذي تصدّر القائمة. في حين حلّ كيفن دورانت في المركز الثامن.",

                    "وعرف اللياقة البدنية Clarke، 1976 على أنها القدرة على القيام بالأعباء اليومية بقوة ووعي وبدون تعب لا مبرر له من توافر قدر كاف ممن الطاقة",
                    "هي السمات البدنية التي على الفرد تحقيقها للقيام بالأعمال اليومية الاعتيادية بأفضل كفاءة دون الشعور بالتعب والإرهاق الشديد. هي الأعمال اليومية على بساطتها، لكنها قد تزداد تعقيدا لتشمل الرياضات التنافسية والنشاطات الأكثر صعوبة.",
                    "المرونة هي قدرة المفصل على الحركة لأبعد مدى ضمن نطاق حركته! ومن الأمثلة على التمارين التي تحسن المرونة تمارين اليوغا وشد الجسم. مثلاً، قم بوضع قدميك على الأرض مع جعل مسافة بينهما",

                    "المِرِّيخ أو الكوكب الأحمر هو الكوكب الرابع من حيث البعد عن الشمس في النظام الشمسي وهو الجار الخارجي للأرض ويصنف كوكبا صخريا",
                    "مذبحة القلعة، أو مذبحة المماليك هي حادثة تاريخيَّة وقعت في ولاية مصر العثمانية دبرها محمد علي باشا للتخلص من أعدائه المماليك يوم الجمعة 5 صفر سنة 1226 هـ الموافق 1 مارس",
                    "اعتقلت الولايات المتحدة الأمريكية أكثر من 779 معتقلاً إدارياً خارج نطاق القضاء في معتقل غوانتانامو الأمريكي في كوبا منذ فتح معسكرات الاعتقال في 11 يناير 2002",
                    "الحركة الثقافية هي كل تغيير في طريقة التفكير والاعتقادات السائدة بخصوص مجموعة من القضايا الثقافية ذات العلاقة بنواحٍ وفروع معينة، مما يترتب عليه تغيير في طريقة مقاربة ومعالجة العمل",
                    "الهندسة التطبيقية تعني تطبيق العلم لتوفير الحاجات الإنسانية. وذلك من خلال تطبيق العلوم النظرية والتطبيقية: الفيزياء، الكيمياء، الرياضيات، والأحياء.",
                    "الإنسان الآلي أوالروبوت (بالإنجليزية: Robot)‏ عبارة عن أداة ميكانيكية قادرة على القيام بفعاليات مبرمجة سلفا ويقوم الإنسان الآلي بإنجاز تلك الفعاليات",
                    "إمحوتب هو بانى هرم زوسر المدرج وهو أول مهندس معمارى ، وأحد أشهر المهندسين في مصر القديمة ، رفع إلى درجة معبود بعد وفاتة وأصبح إله الطب ."
                };

        private static string[] bodyDe =
        {
                    "Ein köstliches und köstliches Gericht mit einer schönen Form, das aus dem Gemüse zubereitet werden kann, das in der arabischen Küche immer vorhanden ist",
                    "Vorspeisen, Vorspeisen, Mezze (in der Levante) oder Moftahat (in Tunesien) ist ein Lebensmittel aus Gemüse oder Obst, das gekocht, eingelegt oder gehackt wird. Es wird normalerweise als Gewürz verwendet, insbesondere um das Hauptgericht zu verschönern.",
                    "Die Gurke wird aus Beizen (Beizen) hergestellt, dh in eine Substanz gegeben, die damit gemischt ist. Die Gurke wird beispielsweise in einem Druckbehälter, der eine Woche lang Luft daraus zieht, in Wasser und Salz eingeweicht.",

                    "Der Mensch erlebte Reisen von Beginn seiner Existenz auf dem Planeten an, als er gezwungen war, sich auf der Suche nach Wasser und Nahrung zu bewegen, und fuhr dann damit fort, nachdem er von den Strapazen der Straße reich geworden war, als die menschliche Seele unerträglich wurde.",
                    "Die positiven und negativen Aspekte des Reisens und der Gefühle werden einerseits von der Natur eines Menschen und andererseits von der Art seiner Reise und seinem Zustand bestimmt. Aus den Nachrichten über die Vorteile der Entfremdung fand er darin einen beruhigenden Zufluchtsort für ihn.",

                    "Fairouz ist eine libanesische Sängerin. Ihr richtiger Name ist  Nihad Rizk Wadih Haddad  . Sie wurde im November 1935 in einem Viertel namens Zuqaq El Blat in Beirut geboren. Sie spielte viele Lieder und Opern.",
                    "Laila Murad ist eine ägyptische Sängerin und Schauspielerin. Sie gilt im 20. Jahrhundert als eine der bekanntesten Sängerinnen und Schauspielerinnen der arabischen Welt. Sie begann ihre Gesangskarriere im Alter von vierzehn Jahren.",
                    "Sayed Darwish ist ein ägyptischer Sänger und Komponist, dessen richtiger Name Herr Darwish Al-Bahr ist. Er ist der Erneuerer der Musik und der Katalysator für die musikalische Renaissance [?] In Ägypten und der arabischen Welt.",

                    "Die Olympischen Sommerspiele oder die Olympischen Sommerspiele sind ein Sportereignis, das alle vier Jahre für die Olympischen Spiele stattfindet.",
                    "LeBron James wurde Erster unter den NBA-Spielern und im Allgemeinen Zweiter nach Cristiano Ronaldo, dem Star von Real Madrid, der die Liste anführte, während Kevin Durant Achter wurde.",

                    "Er definierte Fitness von Clarke, 1976, als die Fähigkeit, tägliche Aufgaben mit Kraft, Bewusstsein und ohne übermäßige Müdigkeit aufgrund der Verfügbarkeit ausreichender Energie auszuführen.",
                    "Sie sind die physischen Merkmale, die ein Individuum erreichen muss, um normale tägliche Aufgaben mit der besten Effizienz zu erledigen, ohne sich müde und erschöpft zu fühlen. Die täglichen Aufgaben sind einfach, aber sie können komplexer werden, um Leistungssport und schwierigere Aktivitäten einzuschließen.",
                    "Flexibilität ist die Fähigkeit des Gelenks, sich innerhalb seines Bewegungsbereichs am weitesten zu bewegen! Beispiele für Übungen, die die Flexibilität verbessern, sind Yoga-Übungen, die den Körper dehnen. Stellen Sie beispielsweise Ihre Füße mit einem Abstand zwischen ihnen auf den Boden.",

                    "Der Mars oder der rote Planet ist der vierte Planet in Bezug auf die Entfernung von der Sonne im Sonnensystem und der äußerste Nachbar der Erde. Er wird als felsiger Planet klassifiziert.",
                    "Das Massaker an der Burg oder das Massaker an den Mamluken ist ein historischer Vorfall in der osmanischen Provinz Ägypten, der von Muhammad Ali Pascha geplant wurde, um seine Mamluk-Feinde am Freitag, dem 5. Safar 1226 AH, entsprechend dem 1. März, loszuwerden.",
                    "Die Vereinigten Staaten von Amerika haben seit der Eröffnung der Haftlager am 11. Januar 2002 mehr als 779 außergerichtliche Verwaltungshäftlinge im Haftzentrum von Guantanamo Bay in Kuba festgenommen.",
                    "Die kulturelle Bewegung ist jede Änderung der Denkweise und der vorherrschenden Überzeugungen in Bezug auf eine Gruppe kultureller Probleme, die sich auf bestimmte Aspekte und Zweige beziehen, was zu einer Änderung der Art und Weise führt, wie wir mit Arbeit umgehen und sie behandeln.",
                    "Angewandte Technik bedeutet, Wissenschaft anzuwenden, um die Bedürfnisse des Menschen durch Anwendung theoretischer und angewandter Wissenschaften zu erfüllen: Physik, Chemie, Mathematik und Biologie.",
                    "Ein Roboter oder ein Roboter (auf Englisch: Roboter) ist ein mechanisches Werkzeug, das vorprogrammierte Aktivitäten ausführen kann, und der Roboter führt diese Aktivitäten aus.",
                    "Imhotep war der Erbauer der Stufenpyramide von Djoser, dem ersten Architekten und einer der berühmtesten Ingenieure im alten Ägypten. Nach seinem Tod wurde er zum Idol erhoben und zum Gott der Medizin."
                };

        public static async Task AddSeedBlogs(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDbContext>();
            Random random = new Random();

            // Add Blogs by Admins --------------------------------------------------
            var admins = await context.Users.Where(u=>u.UserName.Contains("admin") || u.UserName.Contains("editor")).ToListAsync();
            foreach (var admin in admins)
            {
                for (int n = 0; n < 10; n++)
                {

                    for (var i = 0; i < 20; i++)
                    {
                        var randNum = random.Next(1, 100);


                        var title = bodyAr[i].Substring(0, 15);
                        var sTitle = bodyAr[i].Substring(0, 30);
                        var body =  bodyAr[i];
                        var lngId = DefLanguageCodeId[1];
                        var publiched = random.Next(0, 1000) < 990 ? true : false;
                        var top = random.Next(0, 10000) < 2 ? true : false; // to add some of blogs  of Top of the list

                        if (randNum > 33 && randNum < 66)
                        {
                            title = bodyDe[i].Substring(0, 15);
                            sTitle = bodyDe[i].Substring(0, 30);
                            body = bodyDe[i];
                            lngId = DefLanguageCodeId[2];
                        }
                        if (randNum > 66)
                        {
                            title = bodyEn[i].Substring(0, 15);
                            sTitle = bodyEn[i].Substring(0, 30);
                            body = bodyEn[i];
                            lngId = DefLanguageCodeId[0];
                        }

                        // change some relaseDate
                        var daterelase = DateTime.Now;
                        if (randNum == 1)
                            daterelase = DateTime.Now;
                        else if (randNum > 1 && randNum < 10)
                            daterelase = DateTime.Now.AddDays(-5);
                        else if (randNum > 10 && randNum < 20)
                            daterelase = DateTime.Now.AddDays(-10);
                        else if (randNum > 20 && randNum < 30)
                            daterelase = DateTime.Now.AddDays(-15);
                        else if (randNum > 30 && randNum < 40)
                            daterelase = DateTime.Now.AddDays(-20);
                        else if (randNum > 40 && randNum < 50)
                            daterelase = DateTime.Now.AddDays(-25);
                        else if (randNum > 50 && randNum < 60)
                            daterelase = DateTime.Now.AddDays(-30);
                        else if (randNum > 60 && randNum < 70)
                            daterelase = DateTime.Now.AddDays(-35);
                        else if (randNum > 70 && randNum < 80)
                            daterelase = DateTime.Now.AddDays(-40);
                        else if (randNum > 80 && randNum < 90)
                            daterelase = DateTime.Now.AddDays(-45);


                        // add new Blog
                        var newBlog = new Blog
                        {
                            Title = title,
                            ShortTitle = sTitle,
                            Body = body,
                            Publish = publiched,
                            Commentable = true,
                            AtTop = top,
                            ReleaseDate = daterelase,
                            LanguageId = lngId,
                            UserId = admin.Id,
                            UserModifiedId = admin.Id,
                            AddedDateTime = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        context.Blog.Add(newBlog);
                    }
                }
            }
            await context.SaveChangesAsync();
            // END Add Blogs --------------------------------------------------

            // get blogs
            var blogs = context.Blog.Where(b => b.Publish == true).OrderByDescending(b => b.ReleaseDate).ToList();
            // add blogs categories
            foreach (var blog in blogs)
            {
                var catNum = random.Next(1, 4);
                var catIdsList = new List<int>();
                for (int i = 0; i < catNum; i++)
                {
                    int catId = random.Next(1, 7);

                    if (!catIdsList.Contains(catId))
                    {
                        var cat = new BlogCategoryList
                        {
                            BlogId = blog.Id,
                            BlogCategoryId = catId
                        };
                        context.BlogCategoryList.Add(cat);
                        catIdsList.Add(catId);
                    }
                }
            }
            await context.SaveChangesAsync();

            // add Blog images
            foreach (var blog in blogs)
            {
                var indexImage = random.Next(1, 52);
                var repNumber = random.Next(1, 4);
                var defaultImage = true;
                for (int rep = 1; rep <= repNumber; rep++)
                {

                       var blogImage = new UploadBlogImagesList
                    {
                        Name = "img (" + indexImage + "_" + rep + ")",
                        Path = UploadBlogImageDir+ "img(" + indexImage + ").jfif",
                        BlogId = blog.Id,
                        Default = defaultImage,
                        UserId = blog.UserId
                    };
                    defaultImage = false;
                    await context.UploadBlogImagesList.AddAsync(blogImage);
                }
               
            }
            await context.SaveChangesAsync();

            // Add Comments and Likes by visitors --------------------------------------------------
            var users = await context.Users.Where(u => u.UserName.Contains("visitor")).ToListAsync();
            // add like and dislike
            foreach (var user in users)
            {
                // like/dislike number for this user
                var repeatedNum = random.Next(1, blogs.Count()/3);
                for (int i = 0; i < repeatedNum; i++)
                {
                    var like = true;
                    var dislike = false;
                    // select random Blog
                    var randomBlog = blogs[random.Next(1, blogs.Count())];
                    if (random.Next(0, 6) < 3)
                    {
                        like = false;
                        dislike = true;
                    }
                    var likeDislike = new BlogLike
                    {
                        Like = like,
                        Dislike = dislike,
                        AddedDateTime = DateTime.Now,
                        BlogId = randomBlog.Id,
                        UserId = user.Id
                    };
                    await context.BlogLike.AddAsync(likeDislike);
                    
                    if (random.Next(0, 6) < 3)
                    {
                        var comment = new BlogComment
                        {
                            AddedDateTime = DateTime.Now,
                            BlogId = randomBlog.Id,
                            UserId = user.Id,
                            Comment = "Hi I'm " + user.UserName + ", random number : " + i
                        };
                        await context.BlogComment.AddAsync(comment);
                    }
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
