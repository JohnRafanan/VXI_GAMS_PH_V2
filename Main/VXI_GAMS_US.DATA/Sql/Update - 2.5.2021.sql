﻿USE [VXI_GAMS_US]

ALTER TABLE [dbo].[AssignedAssetChangeHistories] ADD [ReturnTrackingNo] [varchar](max)
ALTER TABLE [dbo].[AssignedAssetChangeUploads] ADD [ReturnTrackingNo] [varchar](max)
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'202102041654446_CheckPending0', N'VXI_GAMS_US.DATA.Migrations.Configuration',  0x1F8B0800000000000400ED5D5B6FDCB8157E2FD0FF3098A716C87A7241813618EFC2B193AC5B3B0932E36CFBB49047B4A35A23CDEA62D828FACBFAD09FD4BF50EA428914499194288E344304083CBC7CE7883CE7F07ECEFFFEF3DFE54F4F5B7FF608A2D80B83D3F9AB9397F3190836A1EB05F7A7F334B9FBE1CFF39F7EFCFDEF96EFDDEDD3EC1B2AF7262B076B06F1E9FC7B92ECDE2E16F1E63BD83AF1C9D6DB44611CDE25279B70BB70DC70F1FAE5CBBF2C5EBD5A0008318758B3D9F26B1A24DE16E43FE0CFF330D8805D923AFE75E8023F2ED361CE2A479D7D72B620DE391B703AFFF6F7CB5F3F9E5DAF7EBD599D5C9CADCF4E2E9CC499CFCE7CCF81CCAC807F379F394110264E02597D7B1383551285C1FD6A07131C7FFDBC03B0DC9DE3C7A0FC84B77571D9AF79F93AFB9A455D11416DD23809B78A80AFDE94CDB36856EFD4C8F3AAF96003BE870D9D3C675F9D37E2E9FC2C8E41329F3529BD3DF7A3AC14D9C0DF2EDFFFB23AC9313C109FE4755FCCA8122F2AC9800294FD7B313B4FFD248DC06900D22472FC17B32FE9ADEF6DFE069ED7E103084E83D4F7714621AB308F4880495FA27007A2E4F92BB82BD9BF74E7B305596FD1AC5855C3EA14DFF631F5E0DF9F206DE7D60795182C5AAB9F439944005096A066CC67D7CED31508EE93EFA7F3D77F82BAF0C17B022E4A29516F020F2A922C950B106F226F57F43A9718FC5307B11588A02E7C0A87A774EE24E03E8C9E9B7D20E02FBDED56F1DA09D23B6793895EA44812EA411AABD5F90602378C866FC42F69B4F9EEC4E073E40203E4CEC338F9E6F8699BD4EB12442F3140E5323EDB24DE6345E95D18FAC009942DC1657CE1C5BB30066E5FA47F00A7EAC8CB2079F35A19E13A0C92EFFD20CE230095CC7DF73C7C0FC0411A94E410B12C690D2701CA6CDFEC5C936C97E468B6F1AACB453DD08A87DFC2D6FCECC121387AEE3316134076601E44063E44E176782A6B1363F114F4BD8B26DDECFCD071E3FE9A5402594D1A66B0CFDB78C272AE269B1AA4F258E5F1C0174306BEA95E400D4F0C5F741D8E1531B334B14BC883B0F6DE7D901BECF75B68B19F01E86EF49B484767FBCD6CB7FD12460F198DE1E5F3E7C8730DAC52FCD084193983CA66808AEB46203631530C83040E5C26C6FEF75BC7F30DAC232367F300B14D7CD2DADB3C00238D77702BD7A695EFBD17C401B4A3C7204262470F3B7AD8D1C38E1E23193D7AEF347100EDE861470F3B7AD8D1C38E1E23D85F026E6EA2CFBF3BC1BD96F5020BF0282DBE9913DE8C9289535E935AF815403908ACDE4F65D648A8BC8E59230BF0E86C48663FCCCC1CD7A1213AD6868CDB8648EBFDBBD47FE8A8E159D5FDEAF24D5CDCE255D56754AF874E5F5E18583779519C647F0E4FEACA3145E9DA735D1F98A1F5B317816CA01C9ED24710183937BF003B274AB620480C9841E098B8D808A99850A6AB70939BB6ECEA908921243125E4AB14FE7AF4E23032D18C86B60ACEC334484CDC4D823FFF093606B4E9AFE1ED15780406DAEE4B187B99A41B22579B2433F2FED5091E726843A26142ABF43D4069BC469003929E2DD677063BCD1851F5A35B019AB9B3A44B8A26B1B3C198D54FF9DD4F7DB296A1F6DBBE65621D9DCEE9D286AC53E3C4D9EE86DA7A23BB0B5D5FD6D0F30594EDF8B1763C5A92AC232788EF40D44FEB396847D7FDF6D5DF48F620871DED0F6143B8B3A5E87734C341B39662C2EF840E411DC6783E7215DE7B01FCAFB3AA15D58F4EB7B22316139BBAF7F8FEBEEE091AF970B19300E01047270407E81949D7E2C23A7839D28D9EAF601B3E02E2768C9E473E62E0A3B33F7923941FAFE6B9EBF0AED11CC0A46D2F17E35AD4AADF2A4C0C7C74FA6AB56E845AA7A0293BDFD9F41EC57010AB01836D5E165E6D4C6C619AA2947D95595733EBD02CBD498CACEC8E31E5C7661D9AA2947D9561AF39EBD030C1E2E6BA31573DD9F5F58188A98E627DE77638881DC506F4CB66E0C6B2511B6F1D7E8D769494B621BDEE701CE9AD8D03DC2B5E855175B1B8DB06ADBDD137DD8D5EC2C566374B50231C9F39E814DEC1DEB3B55AD9AE956BB08553F3BE376C1B2847A79DC5C206B8A81D3E783EB8F04CAC804B82EF9CCD43BA3346167DAF39992FE80D75A891DDCE2868F4D3030AC76A8215490591CC7D1C7901886A424E96069E5821C8A0B49542199FCE9308AE2549AE0BCC1548F0E35FB80CAB3938CD03C09D9431CE1612D51BA158D8508D42F2B8D52E531B6E5548065780A88045BBAC6641D2A5D4915B5B975B589D4E6BDB700B4BD1613B5FE2D0611756A723FA1E7661019DCCABC3D9CE6BE9F5C2658400A69CBFC341828150AF8B04289C57511420A79C123ADAB41180A362026CEEDB0E0A9E5B5291025F1AB82585148ACBAB6C4C742F560082EF9EB280C8DD550198CCE5298A824CA5EE74F9CD2E534948973C5267D0200BC8E1B5F14C1610E015CAC0D471493DA9F738D8A682D8451160516B3B0A8D2A214064CC93294C4619D604A89AEA5479CB4511BDB64C582E38616E97D7CE6E07677658D8DB3265B62A62DE9EFFB0528F04BB2D30169B981110B6E2B6A204BFCCB9078DDCD23F5EE6A7269BBADD3AD9F4EFDCDD52C5F0891DA7C9112562EED69CF3D7AD8F8A677F972727ACC8BFE55CAF815337E107F85599EF82FC0341C50C8A834BD59B6531871DDF8918CB88F3D04FB7016F29D256BBD8B7C2EB1729F208C4C6360E4464C8E3D5418F70B03A55E1DBB09D3CE20BB17405BEC8C8AF047364963C6633282C0EDACC53E0B40A184B3059A5CA23A123411C07A5C9A3340EFC70B046968ADC56A77AA4F056C90AED95BF9322DA2A4F9147A8F765094DAC525590EA40AE24569D2E8F56DCF5C7718A1405092DEEFA1382592429F455BDDB4CF4559DAC606DF0ED64C2DAE019F278D896328E8625ABF156ED193779AB3268BCE5A2312434C79E0535F834B69A9A4399FC40476E20681AF45A4165474001C85887C3E279368E50A4C823AC1B43DF5A6DD01B81B2ED5FA0D13242AB407340D5049A0B32568146CB297A2E31B458EE538C340B504FD1312C34E39FD2B327F41DA7F3DCC97CD7A93C7F226F5EF1FA4F6E8F6311302D03451D7E68B15322543973254619EB505787BFC151EA5479A422BC0D8E52A428CC238BD035C444B24892C728E2D2E010458A02028A394380A04495BEA942CA901D5425CB63958E60719C3249618E8DBDC423E6DA58BA025AF5D68EC0AA52EDECBD9B55D3BA2C9504EF66E326B74CB5A6CE9A3A6BEAC663EAB4AE3725C1BB99BAC96D605853674DDDF44CDD5E4D13F39E982ED324032E6D9AE4C086334DF4767F9DAA86D4DCF64769FB127F3AFA158E49E74E45B10E6A0EC1BC683988A2F69D43C8810DA3A87518BCE6B99CEA5C0285B923CFE79451ACA28E77046C5E43EEAF4DF9B56575BD6157E3B5308A2C47DC626046A96B43C942CE107A76A1A46775D83842D1EA6479AC3A2E1C0E55A72A9CCC6071DF8873192C5D610D50457623D60155AA3C128ADC86E3A03495B3BA3A2E1B795457A72B687C1E798DD0F63C450DA12945284DA1F7899869840410392AB62C61090196AC7276B8C3029F918787788ED9D54F15B98C9C4596890A277528301971468712E571EAB86338509DAAC01119578CE08BCCEAA237B45034F314C66214228C188251A2721F34C50B4BDECFE544E6F5362C7934A33876B5BFFF005EDD07501FC4F955C7BA5DA64F5CC6B03AE34E978EF7DE27FBC95A7F2D91C2955019499C61F4479FF463419CC80947953C528928EF18E9160836ACB23CF060AC38681307DEBBD0FE1221892C2114D248631D66ED3DF1962FDBF339CE48F44EDF7EAE247207BD9BDC6970FFFBB0FB96CE03D863ADDFF7EB90EDD21F401761E6551D467A8BD054CD1D5AB57DB17B7AF3B14A1C4DFF92AE17FAF73171855EBD9FDBAB8FD552E97E85A16F2E6B9F958AF08E787B41C2E9497F83A04E44C24C74011DC67834C34711F7B41A79264DD2214D7D749B82F1A99BBEF5833A917EEA36B955C52129C6DE04997028A54368DB00A504B41D60ACC28807596A6E30A174956D2616569DAAC617FF992C9DABC2231FB79937AD51A5AD2D590F73F17495F66361D5A96A7CB53CF56564AB70D9824C65AA5E0F643E256E64A95D14642212196333BE3A670C6D80F2C67772330196B1543795FABD09D857FC239BEF541E2CFB2B5BE7A355B387A9E3DB122BE2E410AA9AA7D8DB32763BABBAEB79ABF52E19EE5EA683C2B6D51E486B394E2FBB79BCB4F7D3E4F08E58E39AAE92FBEB9C005142EF8408031D18F2627C9032C129A472699F19DC86BCC3CF2CA2FE2DB448D7E9AA328D0283D0428D724623D5B4B3EEFE722DC494906C098C69CBF651C81DE5EBBD59A4A25EA654BF2B5FEFA59F75C2017CDE0A993BF7FCEBE3D2E77BD3F17A51643E834DF5E8B999D3F5D5739C806D39E1F9CD3FF7BDFC1D122A00D7B0DE1D889322E6D2FCF5CB57AFE7B333DF73E2C2B37DE952FE6D330E8F948FF9576F321FF3C0DD2E9AD5D53DD5672871EC1221A1E898560C87ED26A246A581F75B0A607B427EEEBC6CCDDD23F0E9A393ADDE2346ECA3CBC0054FA7F37FCD2EE39B9CE2DBD93A4AC1ECDF7480C39A7E1E2349409E1111B5E4E20F5BE7E98FAA70B53BC9FE58740CC8B6C696E28E7411AF07B4E9225E13AB95A3783D78645CE93EDDC28C1CDDAB9F9BB1A17B09607E8FB03F4E33C8E7AD977408DF5BBB8BEF8E52DCEA29EA7B817AFDF24A4F77002A4E699F76654422CD963E899648A47D196BC41A6531464349475D1439549FEA70D5A9B98B7BF6FD71D67A869A7188781751629ECD1C95282177CC7B1503B5AE9B6EA74D60BEA8852FDCFF787F38F2BC704C22AF6BBE742CF34B4326A2DD7DF7242CC59E17B6B56BC9FE9D5E3898D430E929BC4CF6072A7C4D6AC041EE26756859E56FB23F58E97645C3F410BBFAA901ADBAFC7994534E297FDB53B54D9DDADD1A196B64AC9119D4C84C77A9648D8C3532D6C8E85C1289FD4C4FC630E8DAFF440EAAC72660B4C7DB8311DA3D8E8C62FFCD935080DAE9B38E03006D485601345B6DDA79B29C78B25D268B4514D5D324A699874F0D53A5DAF1727FB0DAF5B2864D69CCF5B2869965E57CB93F1672C0ACC132630E9835E85FEE86590F8E1EE1221D31EBB030893E9120FD308F682E5DF963D6705A815C32F787AA9D326B608BF4C8AC5391744947E590595B27E891323D377F1A5749C420D2832AFBE9C924E67D7B3E12D2D1B323593730E63ED3BA0C25E15F781222AD43A630F7B3C32C1EC5CE7B6D5BEB6A6B299FB893686E7BD5CFA8011FCB564167499FEE7E94B69B889A2E538D4514F6B36BC476083B09412A3CC9EA58D6DFE37B29BAC728BE2BD649B4F2613D99D231ABB16F44B81A39AD6591AAF7D349E86BD359AA1856F6E4D30EB6633AA354F5253A09E1B562A6C7AEF1BD781E9518E0EE3F752C89F561D1AE3F75F0A71B71243691DD7CFA1EC1D45E3FF574ACF6473594A74F3D7C6A7DAB4378F91CDAB4D9114E9F29D26D34ECE3347DE23ED9D383C3DA35285C64765FB0DBB3D0F12CFAB9FE24A7A157DA3DEAD86B025635D014AECDE5E324D483EB3C4FC3FC96ED0DB23F30EE954F9794209F7CC36C3D091C281EBBA44CA4432BC789D8404038396CF46AE58B11DF689ECF6A128527C39332BD70910879BDCDF6CD0A1EF3AC98FA622632E9BB8943A651884B132F9739B556E0002D355B39A80A0938A8CAC9D0175016D354A046B9C06012A54B7168370B76E2A1B5EFB9852539529205DE335929CE44FDC42D2FC517F3951E8F2F76613E5F8CF2F22DC67C3E25C599448BB1CB0BF8CA5FC8D01C14C92C5A598E18B6F65D4441D7592C78CCCBBE8806FB06274D90538E459D55549D15E41C49C0092A266604951470C1BBF94733C22DC9E2855D58A6597817B4C40CB58A3BB7B0909DF25E0F8B3ECA62134481AA4524C88D438A0C99CD2245C64C16919338ADA79990A9C4624D584F4624248E6895386E1514997A427E897343166F6401361F5819B95622B6F4B954055F4F9611D0E45AAD363B256999086F7634013C97498588BE2222D65CB2D304A9122CA2642119C2F41A9026CD28C322DE2C4691A75CBD5779CB45015826C09F944BF7E5E26B1A648BA5E2D705C81504412C216600369985AD415199CBE02E44AB31F8E53847A848F32C05240E1CC49CB3082E4BA16583D91B0055327B79951F3C64A3DD2D702F83CF69B24B934C4DB7B73EE18F7FB968A7BF5C503C2F3FE75BDDB18E4F806C7AD938FC3978977ABE5BF1FD81B1BEE440642BC98F00A6E723D72A8932797EAE903E85812450D97C176097BD510D1224A6F1E760E5647B86EABC4161BB02F7CEE6F94BE9999F0F22EE08B2D997179E731F39DBB8C4A8EBC39F5086DDEDD38FFF077B55DCC0E7350100 , N'6.4.4')

ALTER TABLE [dbo].[AssignedAssetChangeHistories] ADD [AssetAssignId] [uniqueidentifier] NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000'
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'202102041745230_CheckPending1', N'VXI_GAMS_US.DATA.Migrations.Configuration',  0x1F8B0800000000000400ED5D5B6FDCB8157E2FD0FF3098A716C87A7241813618EFC2B193AC5B3B0932E36CFBB49047B4A35A23CDEA62D828FACBFAD09FD4BF50EA428914499194288E344304083CBC7CE7883CE7F07ECEFFFEF3DFE54F4F5B7FF608A2D80B83D3F9AB9397F3190836A1EB05F7A7F334B9FBE1CFF39F7EFCFDEF96EFDDEDD3EC1B2AF7262B076B06F1E9FC7B92ECDE2E16F1E63BD83AF1C9D6DB44611CDE25279B70BB70DC70F1FAE5CBBF2C5EBD5A0008318758B3D9F26B1A24DE16E43FE0CFF330D8805D923AFE75E8023F2ED361CE2A479D7D72B620DE391B703AFFF6F7CB5F3F9E5DAF7EBD599D5C9CADCF4E2E9CC499CFCE7CCF81CCAC807F379F394110264E02597D7B1383551285C1FD6A07131C7FFDBC03B0DC9DE3C7A0FC84B77571D9AF79F93AFB9A455D11416DD23809B78A80AFDE94CDB36856EFD4C8F3AAF96003BE870D9D3C675F9D37E2E9FC2C8E41329F3529BD3DF7A3AC14D9C0DF2EDFFFB23AC9313C109FE4755FCCA8122F2AC9800294FD7B313B4FFD248DC06900D22472FC17B32FE9ADEF6DFE069ED7E103084E83D4F7714621AB308F4880495FA27007A2E4F92BB82BD9BF74E7B305596FD1AC5855C3EA14DFF631F5E0DF9F206DE7D60795182C5AAB9F439944005096A066CC67D7CED31508EE93EFA7F3D77F82BAF0C17B022E4A29516F020F2A922C950B106F226F57F43A9718FC5307B11588A02E7C0A87A774EE24E03E8C9E9B7D20E02FBDED56F1DA09D23B6793895EA44812EA411AABD5F90602378C866FC42F69B4F9EEC4E073E40203E4CEC338F9E6F8699BD4EB12442F3140E5323EDB24DE6345E95D18FAC009942DC1657CE1C5BB30066E5FA47F00A7EAC8CB2079F35A19E13A0C92EFFD20CE230095CC7DF73C7C0FC0411A94E410B12C690D2701CA6CDFEC5C936C97E468B6F1AACB453DD08A87DFC2D6FCECC121387AEE3316134076601E44063E44E176782A6B1363F114F4BD8B26DDECFCD071E3FE9A5402594D1A66B0CFDB78C272AE269B1AA4F258E5F1C0174306BEA95E400D4F0C5F741D8E1531B334B14BC883B0F6DE7D901BECF75B68B19F01E86EF49B484767FBCD6CB7FD12460F198DE1E5F3E7C8730DAC52FCD084193983CA66808AEB46203631530C83040E5C26C6FEF75BC7F30DAC232367F300B14D7CD2DADB3C00238D77702BD7A695EFBD17C401B4A3C7204262470F3B7AD8D1C38E1E23193D7AEF347100EDE861470F3B7AD8D1C38E1E23D85F026E6EA2CFBF3BC1BD96F5020BF0E82C7EDE04457B68183BCC9C1567944C9C179BD4E7AF004A54602DC854E69F84F1D031FF64011E9D35CAEC879939E83A3444C7DA9071DB1069BD7F97FA0F1D353CABBA5F5DBE898BFBC0AAFA8CEAF5D0E9CB0B032B302F8A93ECCFE1495D39A6285D7BAEEB0333B47EF622900D94C353FA08022327F01760E744C91604890133081C135724211513CA74156E72D3965D4232318424A6847C95C25F8F5E1C46269AD1D0A6C379980689895B4EF0E73FC1C68036FD35BCBD028FC040DB7D09632F937443E46A936446DEBF3AC1430E6D48344C6895BEA72C8D770D7240D2B3C5FAF661A71923AA7E742B4033B79F7449D124763618B3FA29BF20AACFE832D47E1BC14CACA3D3395DDA90756A9C38DBDD505B6F6477A18BD01A7ABE80B21D3FD68E474B9275E404F11D88FA693D07EDE8BADFBE1F1CC91EE4B0A3FD216C0877B614FD8E663868D6524CF8C5D121A8C318CF47AEC27B2F80FF7556B5A2FAD1E95676C4626253F71EDFDFD73D41239F407612001CE2E884E0007D2CE95A5C58573147BAD1F3156CC34740DC8ED1F35C480C7C74F6276F84F2E3D57C801DDE359A0398B4EDE5625C8B5AF55B8589818F4E5FADD68D50EB143465E73B9BDEA3180E623560B0CDCBC23F8E892D4C5394B2AF32EBB4661D9AA537899195DD31A63CE2AC435394B2AF32EC7F671D1A2658DC5C37E6F427BBBE3E1031D551ACEFDC0E07B1A3D8801EDE0CDC58366AE3ADEBB0D18E92D236A4D71D8E23BDB571807BC5AB30AA2E1677DBA0B537FAA6BBD14B38EBEC66096A84E333079D0245D87BB6562BDBB5720DB6706ADEF7866D03E5E8B4B358D80017B5C307CF07179E89157049F09DB3794877C6C8A2EF3527F305BDA10E35B2DB19058D7E7A40E1584DB022A92092B9B7242F00514DC8C9D2C0132B981994B65228E3D37912C1B524C97581B902097EFC0B97613507A77928B993325ADA42A27A23A80B1BAA51481EB7DA656AC3AD0AC9E00A1015B068E7D72C48BA943A726BEB720BABD3696D1B6E61293A6C374E1C3AECC2EA7444DFC32E2CA093797538DB792DBD5EB88C10C094F37738483010EA75910085F32A8A02E4945342479B360270544C80CD7DDB41C1734B2A52E04B03B7A490427179958D89EEC50A40F0DD531610B9BB2A0093B93C455190A9D49D2EBFD9652A09E99247EA0C1A640139BC369EC90202BC4219983A2EA927F51E07DB5410BB28022C6A6D47A1512504888C793285C928C39A0055539D2A6FB928E2E09609CB052760EEF2DAD9EDE0CC0E0BA05BA6CC5645F4DCF31F56EA3165B705C662133342CB56DC5694E09739F7A0915BFAC7CBFCD46453B75B279BFE9DBB5BAA183EB1E33439A244CCDD9A73FEBAF551F1ECEFF2E4841543B89CEB3570EA26FC00BF2AF35D907F20A898411175A97AB32C7AB1E33B116319711EFAE936E02D45DA6A17FB5678FD22451E81D8D8C681880C79BC3A7C120E56A72A7C1BB693477C2196AEC017194396608ECC92C76C8697C5419B790A9C56A1670926AB547924742488E3A0347994C6811F0ED6C85291DBEA548F14DE2A59A1BDF27752445BE529F208F5BE2CA18955AA0A521D1296C4AAD3E5D18ABBFE384E91A220A1C55D7F42308B2485BEAA779B89BEAA9315AC0DBE9D4C581B3C431E0FDB52C6D1B06435DEAA3DE3266F55068DB75C348684E6D8B3A0069FC656537328931FE8C80D044D835E2BA8EC08280019EB70583CCFC6118A1479847563E85BAB0D7A2350B6FD0B345A466815680EA89A407341C62AD0683945CF258616CB7D8A916601EA293A868566FC537AF684BEE3749E3B99EF3A95E74FE4CD2B5EFFC9ED712C02A665A0A8C30F2D764A842A67AEC428631DEAEA403A384A9D2A8F5404CAC1518A1485796411048798481649F21845841B1CA248514040D16B081094A8D23755701AB283AA6479ACD2112C8E532629CCB1B19778C45C1B4B5740ABDEDA115855AA9DBD77B36A5A97A592E0DD6CDCE496A9D6D45953674DDD784C9DD6F5A62478375337B90D0C6BEAACA99B9EA9DBAB6962DE13D3659A64C0A54D931CD830A6A911EB8F5026324BCDDCD1470875AA1A52F32801A5ED4BA5E8885A38269D3B15653DA87909F3F2E620CADF775E2207368CF2D7A1F59A677DAAF313143A8F3CF35346B18A3ADE51B579B5B9BF36E557A1D5F5865D8DD7C2285A1D71338219F9AE0D250B6343E8D985929ED5A1E80845AB93E5B1EA587338549DAA70DA83C59223CE7AB074857545152D8E585B54A9F248281A1C8E83D254CEFFEA586FE4F15F9DAEA0F179343742DBF3143584A614A13485DE27E2B0111240E4A8D8B284250458B2CA79E40E0BA6461E48E239665754553434721659262A9CFEA16067C4B91F4A94C7A96399E14075AA024764AC32822F32AB8BDED042D1CC53188B51D83162084689CA7DD0142F2C793F171E9957E6B0E4D18CE2D87381FE037875C7407D10E7571DEB169C3E7119C3EA8C3B5D3ADEBBA4EC6770FDB5440A574265247186D11F7DD28F058622271C55F24825A2BCB7A45B20D8B0CAF2C083B1E2A04D1C786F4DFB4B8424B2845048238D7598B577CF5BBE6CCF674323D13B7DFBB992C81DF46E7227CCFDEFD8EE5B3A0F608FB5F619A043B64B1F035D8499577518E92DC25D357768D5F6C5EEE9CDC72A7134FD4BBA73E8DFC7C4B57CF57E6EAF3E564BA5FB6587BEB9AC7DAA2AC23BE2ED0509472AFD0D823A110933D1057418E3D10C49455D57C1F24C9AA4439AFAE83605E353377DEB077522FDD46D72AB8A43528CBD0932E1A44A87D0B6014A09683BC05885110FDCD4DC6042E92ADB4C2CAC3A558D2FFED35B3A5785473E6E336F5AA34A5B5BB21EFBE2E92AEDC7C2AA53D5F86A793ECCC856E1B20599CA54BD1EC87C9EDCC852BB28C8442432C6667C75CE18DA00E58DEFE466022C63A96E2AF57B28B09E014636DFA9BC62F657B6CE47AB660F53C7B72556C4DE2154354FB1B765EC765675D7F356EB5D32DC654D07856DAB3D90D6721C6976F3A269EFA7C9E11DB1C635DD2FF7D73901A284DE0911063A30E4C50D2165825348E5D23E33600E79879F5944FD5B6891AED355651A051BA1851AE58C46AA6907E0FDE55A882921D91218D396EDA3903BCA7F7CB34845BD4CA97E57FEE34BDFED8453F9BC153217F1F9D7C7A51FF9A633F7A2C87C069BEAD1733347EEABE73801DB72C2F39B7FEE7BF93B245400AE61BD3B1027451CA7F9EB97AF5ECF6767BEE7C485B7FCD24DFDDB666C1F29BFF5AFDE647EEB81BB5D34ABAB7BBFCF50E2D825C24CD171B2184EE04D44A24A03EFB714C0F684FCDC79D99ABB4730D547275BBD478C784A97810B9E4EE7FF9A5DC63739C5B7B3759482D9BFE9A08935FD3CEE92803C23CA6AC9C51FB6CED31F55E16A1795FDB1E8B8926D8D2DC51DE9765E0F68D3EDBC26562BE7F37AF0C858D57DBA85198DBA573F37E34DF712C0FC1E617F9C66E0D05B2FE91012B87641DF1DA5B8D553D4F702F5FAE5959EEE0054ECD33EEDCA886E9A2D7D122DD14DFB32D6885FCA628C86928EE42872D23ED5E1AA537317F7ECFBE3ACF50C35E310F12EA2C43C9B392A51422E9EF72A066A5D37DD4E9BC07C510B5FB84FF3FE70E479E198445ED77CE958E697864C44BB4BF049588A3D2F6C6B7795FD3BBD705AA961D25378AEEC0F54F8AFD480835C58EAD0B2CA87657FB0D2ED8A86E92176F553035A75F9F328A79C523EBCA76A9B3AB5BB3532D6C8582333A89199EE52C91A196B64AC91D1B92412FBAE9E846168F8BAD6686E74EDAA22B7D763135BDA8FEEC1A8C21EC75BB157E849A855ED4A5AC7B1823624AB009AC702DA25B39C78B21D318B4514D5D324A699DF500D13B0DA9D737FB0DAA1B386AD6ECCA1B386F96AE5D2B93F1672EBACC132636E9D35E85FEEDC590F8E1EE122DD3BEBB030893E9120BD3B8F68865E7979D67006821C3DF787AA5D3D6B608BF4F3AC5391744947E5E6595B27E891323DF7891A1754C420D2832AFB41CB24E67D7B3E68D2D1B323593730E63ED3BA6225E1B5781222AD43A630A7B6C32C1EC52E816D5BEB6A6B294FBB93686E7B81D0A8011FCB564167499FEE7E94B6FB8D9AAE688D4514F6B36BC476333B09412AFCD3EA58D6DFE37B29BAC728BE83D749B4F2613DC4D231ABB12F4FB81A39AD6591AA4FD549E86BD305AB1856F6E4D30EB6633AA354F5503A09E1B562A6C7AEF17D831E9518E04E45752C89F561D10E4575F0A71B71243691DD7CFA9ED6D4BE44F574ACF6A73A94FF503D7C6A7D0144F80E1DDAB4D9114E9F29D26D34EC93377DE23ED9D383C3DA35281C6F765FB0DBB3D0F12CFAB95E2AA7A157DAFDF4D86B025635D014AECD91E424D483EB924FC3FC96ED63B23F30EEEB4F9794204F7FC36C3D09DC321EBBA44CA4432B778CD84040B84E6CF46AE5E111DF689ECF6A12857FC49332BD70BC0879BDCDF6CD0A1EF3AC98FA622632E9118A43A651884B132F97B9CA56E0002D355B39A80A0938A8CAC9D0175016D354A04639D66012A54B7168370B76E2A1B5EFB9852539529205DEE35B29CE44FDC42D2FC517F3ED1F8F2F76613E5F8CF2F22DC67C3E25C599448BB1CB0BF8CA5FC8D01C14C92C5A598E18B6F6884441D7592C78CC77BF8806FB06274D90538E459D55549D15E47249C0092A266604951470C1BBF94733C22DC9E2855D58A6597817B4C40CB58A3BB7B0909DF25E0F8B3ECA621344E1AF4524C88D438A0C99CD2245466216919338ADA79990A9C4624D584F4624248E6895386E1514997A427E897343166F6401361F5819B95622B6F4B954055F4F9611D0E45AAD363B256999081F7934013C97498588E92222D65CB2D304A9122CA2642119C2F41A9026CD28C322DE2C4691A71CC85779CB45015826C09F94A3F8E5E26B1A648BA5E2D705C81504412C216600369985AD415199CBE02E44AB31F8E53847A848F32C05240E1CC49CB3082E4BA16583D91B0055327B79951F3C64A3DD2D702F83CF69B24B336F07607BEB135EFE978B76FACB05C5F3F273BED51DEBF804C8A6978DC39F8377A9E7BB15DF1F18EB4B0E44B692FC08607A3E72AD922893E7E70AE95318480295CD770176D91BD52041621A7F0E564EB667A8CE1B14B62B70EF6C9EBF94FEFEF920E28E209B7D79E139F791B38D4B8CBA3EFC0965D8DD3EFDF87FA52A90BBDB360100 , N'6.4.4')

INSERT INTO dbo.SubCategories
(
    Id,
    CategoryId,
    Code,
    IsActive,
    CreatedBy,
    DateCreated
)
VALUES
(   
NEWID(),      -- Id - uniqueidentifier
'8083F8EC-060C-4FA4-8D72-36C388CC8BDD',      -- CategoryId - uniqueidentifier
'THIN CLIENT',        -- Code - varchar(250)
1,      -- IsActive - bit
'PH-214308',        -- CreatedBy - varchar(max)
GETDATE()
)

DROP PROCEDURE IF EXISTS [dbo].[spAssignedAssetChangeUploads]
GO
/****** Object:  StoredProcedure [dbo].[spAssignedAssetChangeUploads]    Script Date: 1/27/2021 9:32:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spAssignedAssetChangeUploads]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[spAssignedAssetChangeUploads] AS' 
END
GO
ALTER PROCEDURE [dbo].[spAssignedAssetChangeUploads]
(
@createdBy VARCHAR(10) = NULL
)
AS
BEGIN

		SET NOCOUNT ON;

		BEGIN TRY	

			DECLARE @date DATETIME = GETDATE()
			DECLARE @InvalidCodes VARCHAR(MAX) = NULL
			DECLARE @MSG VARCHAR(2048) = NULL
			BEGIN TRANSACTION
	
			IF OBJECT_ID('tempdb..#InvalidCodes') IS NOT NULL 
			BEGIN
				DROP TABLE #InvalidCodes
			END

			SELECT a.FromCode[Code] INTO #InvalidCodes FROM dbo.AssignedAssetChangeUploads a WHERE a.CreatedBy = @createdBy
			AND NOT EXISTS(SELECT 1 FROM dbo.Assets b WHERE b.Code = a.FromCode)

			SET @InvalidCodes = (SELECT (',[' + Code + ']') AS [text()] FROM #InvalidCodes FOR XML PATH(''))

			IF @InvalidCodes IS NOT NULL
			BEGIN
				SET @MSG = FORMATMESSAGE('CANNOT FIND ASSET %s', STUFF(('' + @InvalidCodes),1,1,''));
				RAISERROR (@MSG,16,1);
			END

			TRUNCATE TABLE #InvalidCodes
			INSERT INTO #InvalidCodes (Code)
			SELECT a.ToCode FROM dbo.AssignedAssetChangeUploads a WHERE a.CreatedBy = @createdBy
			AND NOT EXISTS(SELECT 1 FROM dbo.Assets b WHERE b.Code = a.ToCode)

			SET @InvalidCodes = (SELECT (',[' + Code + ']') AS [text()] FROM #InvalidCodes FOR XML PATH(''))

			IF @InvalidCodes IS NOT NULL
			BEGIN
				SET @MSG = FORMATMESSAGE('CANNOT FIND ASSET %s', STUFF(('' + @InvalidCodes),1,1,''));
				RAISERROR (@MSG,16,1);
			END

			TRUNCATE TABLE #InvalidCodes
			INSERT INTO #InvalidCodes (Code)
			SELECT a.FromCode FROM dbo.AssignedAssetChangeUploads a WHERE a.CreatedBy = @createdBy
			AND NOT EXISTS(SELECT 1 FROM dbo.AssignAssetEmployees b WHERE b.Code = a.FromCode)

			SET @InvalidCodes = (SELECT (',[' + Code + ']') AS [text()] FROM #InvalidCodes FOR XML PATH(''))

			IF @InvalidCodes IS NOT NULL
			BEGIN
				SET @MSG = FORMATMESSAGE('CANNOT FIND ASSIGNMENT OF CODE %s', STUFF(('' + @InvalidCodes),1,1,''));
				RAISERROR (@MSG,16,1);
			END
			
			IF OBJECT_ID('tempdb..#ChangeTmp') IS NOT NULL 
			BEGIN
				DROP TABLE #ChangeTmp
			END

			/********start GET AssignedAsset Id*******/
			SELECT 
			NEWID()[Id],
			NEWID()[AssignId],
            FromCode,
            ToCode,
            CreatedBy,
			@date[DateCreated],
			TrackingNo,
			TicketNo,
			ReturnTrackingNo
			INTO #ChangeTmp
			FROM dbo.AssignedAssetChangeUploads WHERE CreatedBy = @createdBy

			UPDATE a
			SET a.AssignId = b.Id
			FROM #ChangeTmp a
			INNER JOIN dbo.AssignAssetEmployees b ON b.Code = a.FromCode
			/********end GET AssignedAsset Id*******/

			UPDATE a
			SET a.Code = b.ToCode, a.TrackingNo = b.TrackingNo, a.TicketNo = b.TicketNo
			FROM dbo.AssignAssetEmployees a
			INNER JOIN dbo.AssignedAssetChangeUploads b ON b.FromCode = a.Code
			WHERE b.CreatedBy = @createdBy
					 
			INSERT INTO dbo.AssignedAssetChangeHistories		 (Id,
			                                                      CodeFrom,
			                                                      CodeTo,
			                                                      CreatedBy,
			                                                      DateCreated,
																  TrackingNo,
																  TicketNo,
																  ReturnTrackingNo,
																  AssetAssignId)
			SELECT 
			NEWID(),
            FromCode,
            ToCode,
            CreatedBy,
			DateCreated,
			TrackingNo,
			TicketNo,
			ReturnTrackingNo,
			AssignId
			FROM #ChangeTmp
			
			DELETE FROM dbo.AssignedAssetChangeUploads WHERE CreatedBy = @createdBy

			COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
					ROLLBACK TRANSACTION;
					THROW;
		END CATCH

END
GO