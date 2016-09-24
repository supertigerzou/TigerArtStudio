USE [tigerstudio_db]
GO

--INSERT INTO [dbo].[MorningNightSharing]
           ([Date]
           ,[IsMorning]
           ,[Description]
           ,[Author]
		   ,[Title]
		   ,[Type])
     VALUES
           ('2016-09-24'
           ,1
           ,N'我是老虎工作室的静子 美好的一天 从此刻开始 我们经常会产生一种错觉，就是认识了许多厉害的人之后，自己也会变得厉害。而事实上不过是吹嘘的资本，在一次次吹嘘中不仅欺骗了别人，更蒙蔽了自己。<br/><br/>一次偶然的机会，我拿到了一位自己一直非常喜欢的作家的微信。为了能够跟心目中的偶像进一步接触，甚至抱有一丝幻想，能和人家成为“平起平坐”的朋友，我立马迫不及待地发出了添加微信好友的申请。 等待回复的过程显得忐忑而漫长。一方面希望人家按下“同意”键，一方面又担心着一旦对方通过，自己不知道该以怎样的方式去“搭讪”。 半天之后，突然弹出一条消息：“对方已通过您的好友申请。” 我望着空荡荡的对话框，几乎不敢相信自己的眼睛。“这就是我超级喜欢的那个作家？我现在是他的微信好友了哇！”  <br/><br/>那是一种什么感觉呢？ 就是恨不得马上参加一场同学聚会，然后在觥筹交错间云淡风轻地炫耀一句：“你们刚才谈论的那个作家，我有他微信哦。” 很显然，我幻想的事情根本没有出现。 我既没有和偶像热络地攀谈，也更没有什么一见如故成为挚友。相当糟糕的是，由于我连个招呼都不敢打，我们自加了微信之后，就再也没有任何交流。 ——也并非是我不想打招呼。只不过是因为，我根本不知道该怎样介绍自己。在“偶像”面前，我才突然意识到，自己没有做过任何一件拿得出手的事情，能在此刻成为自己的名片。 唯一的变化恐怕就是，我可以随时看到偶像的朋友圈了。他昨天去了哪个城市旅行，今天又吃了些什么，偶尔的挣扎和调皮——我都能看得到。这大概也是唯一能让我觉得自己离偶像很近的事情了。 随着微信里的各路大神越来越多，才发现原来大神之间都是互相认识的。今天新加了一位特别厉害的作者，刷朋友圈的时候，看到人家和别的“大神们”，很早以前就一直在评论区互相打趣儿了。  微妙的是，越厉害的人，发朋友圈的时候，越是“无所顾忌”。考虑到他们的朋友圈连我都能看得到，那大概就是人家没有屏蔽任何人吧。反倒是我这种普罗大众，在微信里有了越来越多大神之后，变得越来越不敢发朋友圈了。 “屏蔽”这个功能的发明，其实就像一面镜子，映射出你心中的软肋。真正强大的人，从来不会屏蔽任何人。只有那些想要靠朋友圈伪装强大的人，才会在每一次都小心翼翼地“分组可见”。 当别人作为嘉宾出席各类高大上的论坛、在朋友圈里宣传自己的新书，自己每天发的动态，无非是今儿又发现了一个特别好吃的火锅，或者终于通过了一门考试……跟别人比起来，自己就是一个彻头彻尾大写加粗的Low。　 所以到后来我就干脆把朋友圈里所有的大神都屏蔽了。 不仅如此，看到人家发朋友圈，甚至连赞都不敢点。或许不要冒泡，自己还能安安稳稳地潜伏在别人的朋友圈里，一旦哪天点个赞，让人家看到是个不认识的人，估计就会被直接删好友了吧？ 我就这样以怂得不能再怂的姿态沦为了一个伟大的“旁观者”。  <br/><br/>我们经常会产生一种错觉，就是当认识了许多厉害的人之后，自己也会变得更厉害。而事实上，这些不过是被自己当作吹嘘的资本，在一次次的吹嘘中，不仅欺骗了别人，更蒙蔽了自己。   原来真的有很多人，是靠不断地扩充自己的微信好友数量，来追求一种虚妄的成就感。 就像是在网络尚不发达的年代，总会有人热衷于收集各种名片。他们不仅用充满功利的态度把所有交情都量化计算为“人脉”，而且愚蠢地认为所谓“人脉”，不过是一个联系方式。 事实上每个人心里都很清楚，在你真的遭遇挫折深夜无助的时刻，通讯录里能放心去“打扰”的人，屈指可数。 《欢乐颂》里，樊胜美削尖了脑袋想要挤进上流社会，每天热衷于富豪们之间的各种高端聚会。那些挥金如土的大佬们平日里对她满脸笑意，而当她家里真的出事的时候，曾经和她“关系很近”的男人们，却一个都没有出现。就连樊胜美自己也压根没有想过向他们求助，因为她心里比谁都清楚，之前脸上所有的笑意，不过是逢场作戏。 <br/><br/>即使的确残酷，我们依然不得不承认：很多时候，即使你看得到别人光鲜亮丽的生活，甚至和人家坐在同一张桌子上共进晚餐，也仍旧无法融入他们的生活。  <br/><br/>从这个角度而言，恰好印证了为什么很多时候，你的“人脉”越宽广，内心却越悲凉。 不要轻易就想要和那些完全与你不在同一世界的人成为微信好友。因为某种程度上，对方通过你的好友申请，对你而言很可能是一件残酷的事情。从你抱着乞求的态度去加好友的那一刻起，就早已注定了两个人之间不可逾越的鸿沟。你或许永远都是美好的旁观者，却终难成为美好本人。 所有的距离，本质上都是人本身的差距。<br/><br/>一位朋友在暑期交流活动中，意外拿到了他心目中一位女神的微信，如获至宝。他本打算以此为突破口，为自己塑造一把男版灰姑娘的爱情故事。可是一个月后便放弃。 他说：“加了女神微信，只不过让我更加确信，自己恐怕永远都配不上她。她也不可能会爱上我。” 无论是门当户对的爱情还是志同道合的友谊，无不是高山流水遇知音的心照不宣，和旗鼓相当棋逢对手的华山论剑。 你的能力决定你的交际。除此之外的一切攀谈，不过是路人甲乙之间的一场搭讪。我是静子 更多精彩不要忘记关注微信公众号“老虎工作室”'
           ,N'静子姐姐'
		   ,N'♪【晚安分享】对方已经通过你的微信好友，然后呢'
		   ,'MorningNightSharing')
GO

--INSERT INTO [dbo].[MorningNightSharing]
           ([Date]
           ,[IsMorning]
           ,[Description]
           ,[Author]
		   ,[Title]
		   ,[Type]
		   ,[AudioName])
     VALUES
           ('2016-09-24'
           ,null
           ,N'<span style="color:blue; font-weight:bold">【团购预告】</span><br/>周日晚8点开团磁力片高端品牌MagSpace人气和性价比最高的三款产品，适合1岁以上的孩子及成人，团购价低到您想象不到，会员价更是无与伦比。同样提供配套的电子大礼包，同时配套伴玩视频课程，不管您家的宝宝是1岁、5岁、10岁，都会有不同的玩儿法哟。<br/><br/>具体的价格信息如下：46片嘉年华，原价958元，天猫价528元，老虎工作室团购价328元，会员价<span style="color:deeppink; font-weight:bold">311</span>元；64片宠物精灵，原价1241元，天猫价646元，老虎工作室团购价388元，会员价<span style="color:deeppink; font-weight:bold">368</span>元；97片潘多拉之恋，原价2273元，天猫价1188元，老虎工作室团购价699元，会员价<span style="color:deeppink; font-weight:bold">664</span>元。<br/><br/>而且，天猫购买是不提供任何额外赠品的，以97片为例，老虎工作室独家赠送摩天轮支架以及厚达84页的数学游戏练习簿一套三册。点击页面左下方<span style="color:blue; font-weight:bold">MagSpace磁力片</span>了解详情。'
           ,N'Helen姐姐'
		   ,N'♪【伴读】《牛津阅读树》二阶第十四课 - Picnic Time'
		   ,'EnglishClass'
		   ,'Oxford_Level2_PicnicTime')
GO

--INSERT INTO [dbo].[MorningNightSharing]
           ([Date]
           ,[IsMorning]
           ,[Description]
           ,[Author]
		   ,[Title]
		   ,[Type]
		   ,[AudioName])
     VALUES
           ('2016-09-23'
           ,null
           ,N'<span style="color:deeppink; font-weight:bold">【截团通知】</span><br/>《兰登英语》1-3阶今晚截团，原价均为820元，工作室团购价均为316元，会员价均为300.2元。此次兰登团购是今年最后一次安排，有需求的家庭务必收全。<br/><br/><span style="color:deeppink; font-weight:bold">【会员招募】</span><br/>老虎工作室第二期会员火热招募中，会员期从2016/10/1至2017/3/31，现在加入立刻开通会员服务，即日起即可享受：<br/><br/>★ 全场书籍95折上折优惠<br/>★ 英文伴读课程高清音频直接下载，在任何播放设备上陪同孩子反复收听学习<br/>★ 孩子生日当天由指定的主播姐姐在老虎电台送出贴心的生日祝福<br/>★ 老虎电台所有故事专辑的高清音频可供下载到故事机及其他播放设备收听<br/>★ 老虎工作室父母课堂及幼儿综合素质培养系列课程的可供下载音频包<br/>★ 其他一些即将推出的服务。。。<br/><br/>点击页面右下方<span style="color:deeppink; font-weight:bold">加入会员</span>购买成功后发送手机号到微信<span style="color:deeppink; font-weight:bold">13611784306</span>（勿打电话和短信）完成会员对接。'
           ,N'月亮姐姐'
		   ,N'♪【伴读】《鹅妈妈童谣》第十四课 - My shining star'
		   ,'EnglishClass'
		   ,'MotherGoose_MyShiningStar')
GO

--INSERT INTO [dbo].[MorningNightSharing]
           ([Date]
           ,[IsMorning]
           ,[Description]
           ,[Author]
		   ,[Title]
		   ,[Type]
		   ,[AudioName]
		   ,[InsidePagePictures])
     VALUES
           ('2016-09-22'
           ,null
           ,N'<span style="color:deeppink; font-weight:bold">【团购通知】</span><br/>《兰登英语》1-3阶火热团购中，原价均为820元，工作室团购价均为316元，会员价均为300.2元。此次兰登团购是今年最后一次安排，有需求的家庭务必收全。<br/><br/><span style="color:deeppink; font-weight:bold">【续费通知】</span><br/>各位会员家长，本期会员将于月底统一到期，希望您能尽快续费，会员期内续费有如下三点优势：<br/><br/>★ 可以走会员通道购买会员服务，享受<span style="color:deeppink; font-weight:bold">95折优惠</span>。<br/>★ <span style="color:deeppink; font-weight:bold">连续会龄</span>是会员的一项重要指标，如果您在会员期结束被移除出群后再重新对接入群的话，会龄会被做清零处理，因此没有办法参加我们即将推出的一些针对老会员的优惠活动。<br/>★ 会员期满之后的几天因为数据要做整体迁移，新会员对接的速度会比较缓慢，您可能会因此错过<span style="color:deeppink; font-weight:bold">3-7天的课程</span>。会员到期被移除出群会丢失之前的历史记录，一定程度内影响到孩子学习的连续性和完整性。<br/><br/>续费步骤很简单：进入会员通道后选择类别“<span style="color:deeppink; font-weight:bold">会员服务</span>”，直接购买即可。'
           ,N'月亮姐姐'
		   ,N'♪【伴读】《兰登英语》第一阶段第十九课 - Bear Hugs'
		   ,'EnglishClass'
		   ,'StepIntoReading_BearHugs'
		   ,'StepIntoReading_BearHugs_1.jpg,StepIntoReading_BearHugs_2.jpg,StepIntoReading_BearHugs_3.jpg')
GO

--INSERT INTO [dbo].[MorningNightSharing]
           ([Date]
           ,[IsMorning]
           ,[Description]
           ,[Author]
		   ,[Title]
		   ,[Type]
		   ,[AudioName])
     VALUES
           ('2016-09-21'
           ,null
           ,N'<span style="color:deeppink; font-weight:bold">【团购通知】</span><br/>《兰登英语》1-3阶火热团购中，原价均为820元，工作室团购价均为316元，会员价均为300.2元。此次兰登团购是今年最后一次安排，有需求的家庭务必收全。<br/><br/><span style="color:deeppink; font-weight:bold">【会员招募】</span><br/>老虎工作室第二期会员火热招募中，会员期从2016/10/1至2017/3/31，现在加入立刻开通会员服务，即日起即可享受：<br/><br/>★ 全场书籍95折上折优惠<br/>★ 英文伴读课程高清音频直接下载，在任何播放设备上陪同孩子反复收听学习<br/>★ 孩子生日当天由指定的主播姐姐在老虎电台送出贴心的生日祝福<br/>★ 老虎电台所有故事专辑的高清音频可供下载到故事机及其他播放设备收听<br/>★ 老虎工作室父母课堂及幼儿综合素质培养系列课程的可供下载音频包<br/>★ 其他一些即将推出的服务。。。<br/><br/>点击页面右下方<span style="color:deeppink; font-weight:bold">加入会员</span>购买成功后发送手机号到微信<span style="color:deeppink; font-weight:bold">13611784306</span>（勿打电话和短信）完成会员对接。'
           ,N'月亮姐姐'
		   ,N'♪【伴读】《卡尔The Very系列》第二课 - The Very Busy Spider'
		   ,'EnglishClass'
		   ,'TheVery_TheVeryBusySpider')
GO

--INSERT INTO [dbo].[MorningNightSharing]
           ([Date]
           ,[IsMorning]
           ,[Description]
           ,[Author]
		   ,[Title]
		   ,[Type]
		   ,[AudioName]
		   ,[InsidePagePictures])
     VALUES
           ('2016-09-18'
           ,null
           ,N'<span style="color:deeppink; font-weight:bold">【团购通知】</span><br/>9/18晚8点兰登3阶开团，同时补团1阶2阶，原价均为820元，工作室团购价均为316元，会员价均为300.2元。此次兰登团购是今年最后一次安排，有需求的家庭务必收全。<br/><br/><span style="color:deeppink; font-weight:bold">【会员招募】</span><br/>老虎工作室第二期会员火热招募中，会员期从2016/10/1至2017/3/31，现在加入立刻开通会员服务，即日起即可享受：<br/><br/>★ 全场书籍95折上折优惠<br/>★ 英文伴读课程高清音频直接下载，在任何播放设备上陪同孩子反复收听学习<br/>★ 孩子生日当天由指定的主播姐姐在老虎电台送出贴心的生日祝福<br/>★ 老虎电台所有故事专辑的高清音频可供下载到故事机及其他播放设备收听<br/>★ 老虎工作室父母课堂及幼儿综合素质培养系列课程的可供下载音频包<br/>★ 其他一些即将推出的服务。。。<br/><br/>点击页面右下方<span style="color:deeppink; font-weight:bold">加入会员</span>购买成功后发送手机号到微信<span style="color:deeppink; font-weight:bold">13611784306</span>（勿打电话和短信）完成会员对接。'
           ,N'Helen姐姐'
		   ,N'♪【伴读】《粉红猪小妹第三辑》第三课 - Tooth Fairy'
		   ,'EnglishClass'
		   ,'PeppaPig3_ToothFairy'
		   ,'PeppaPig3_ToothFairy_1.jpg,PeppaPig3_ToothFairy_2.jpg')
GO

--INSERT INTO [dbo].[MorningNightSharing]
           ([Date]
           ,[IsMorning]
           ,[Description]
           ,[Author]
		   ,[Title]
		   ,[Type]
		   ,[AudioName]
		   ,[InsidePagePictures])
     VALUES
           ('2016-09-24'
           ,null
           ,N'<span style="color:blue; font-weight:bold">【团购预告】</span><br/>周日晚8点开团磁力片高端品牌MagSpace人气和性价比最高的三款产品，适合1岁以上的孩子及成人，团购价低到您想象不到，会员价更是无与伦比。同样提供配套的电子大礼包，同时配套伴玩视频课程，不管您家的宝宝是1岁、5岁、10岁，都会有不同的玩儿法哟。<br/><br/>具体的价格信息如下：46片嘉年华，原价958元，天猫价528元，老虎工作室团购价328元，会员价<span style="color:deeppink; font-weight:bold">311</span>元；64片宠物精灵，原价1241元，天猫价646元，老虎工作室团购价388元，会员价<span style="color:deeppink; font-weight:bold">368</span>元；97片潘多拉之恋，原价2273元，天猫价1188元，老虎工作室团购价699元，会员价<span style="color:deeppink; font-weight:bold">664</span>元。<br/><br/>而且，天猫购买是不提供任何额外赠品的，以97片为例，老虎工作室独家赠送摩天轮支架以及厚达84页的数学游戏练习簿一套三册。点击页面左下方<span style="color:blue; font-weight:bold">MagSpace磁力片</span>了解详情。'
           ,N'月亮姐姐'
		   ,N'♪【伴读】《饼干狗》第六课 - Biscuit Visits the Big City'
		   ,'EnglishClass'
		   ,'Biscuit_BiscuitVisitstheBigCity'
		   ,'Biscuit_Biscuit_1.jpg,Biscuit_Biscuit_2.jpg,Biscuit_Biscuit_3.jpg,Biscuit_Biscuit_4.jpg')
GO