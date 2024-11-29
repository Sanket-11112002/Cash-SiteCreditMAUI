; ModuleID = 'marshal_methods.arm64-v8a.ll'
source_filename = "marshal_methods.arm64-v8a.ll"
target datalayout = "e-m:e-i8:8:32-i16:16:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [449 x ptr] zeroinitializer, align 8

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [898 x i64] [
	i64 17802880886401652, ; 0: tr/Microsoft.VisualStudio.Threading.resources.dll => 0x3f3fa037366a74 => 405
	i64 24362543149721218, ; 1: Xamarin.AndroidX.DynamicAnimation => 0x568d9a9a43a682 => 268
	i64 33071413975074826, ; 2: Microsoft.VisualStudio.RemoteControl => 0x757e469a34480a => 205
	i64 98382396393917666, ; 3: Microsoft.Extensions.Primitives.dll => 0x15d8644ad360ce2 => 194
	i64 120698629574877762, ; 4: Mono.Android => 0x1accec39cafe242 => 172
	i64 131669012237370309, ; 5: Microsoft.Maui.Essentials.dll => 0x1d3c844de55c3c5 => 199
	i64 177794975121655083, ; 6: ja/Microsoft.VisualStudio.Utilities.resources.dll => 0x277a7967ef9c52b => 414
	i64 196720943101637631, ; 7: System.Linq.Expressions.dll => 0x2bae4a7cd73f3ff => 59
	i64 210390243030528795, ; 8: ru/Microsoft.ServiceHub.Resources.dll => 0x2eb74cfb40af31b => 378
	i64 210515253464952879, ; 9: Xamarin.AndroidX.Collection.dll => 0x2ebe681f694702f => 254
	i64 229794953483747371, ; 10: System.ValueTuple.dll => 0x330654aed93802b => 152
	i64 232391251801502327, ; 11: Xamarin.AndroidX.SavedState.dll => 0x3399e9cbc897277 => 296
	i64 233177144301842968, ; 12: Xamarin.AndroidX.Collection.Jvm.dll => 0x33c696097d9f218 => 255
	i64 279670469376841020, ; 13: zh-Hant\Microsoft.VisualStudio.Composition.resources => 0x3e196cf655f953c => 394
	i64 295915112840604065, ; 14: Xamarin.AndroidX.SlidingPaneLayout => 0x41b4d3a3088a9a1 => 299
	i64 311281864840114177, ; 15: ja\Microsoft.VisualStudio.Utilities.resources => 0x451e534f2d0f001 => 414
	i64 316157742385208084, ; 16: Xamarin.AndroidX.Core.Core.Ktx.dll => 0x46337caa7dc1b14 => 262
	i64 350667413455104241, ; 17: System.ServiceProcess.dll => 0x4ddd227954be8f1 => 133
	i64 354178770117062970, ; 18: Microsoft.Extensions.Options.ConfigurationExtensions.dll => 0x4ea4bb703cff13a => 193
	i64 390014653889418737, ; 19: ja/Microsoft.VisualStudio.Validation.resources.dll => 0x5699c42e64695f1 => 427
	i64 408699061380389292, ; 20: Microsoft.VisualStudio.Utilities => 0x5abfda1859d19ac => 209
	i64 422779754995088667, ; 21: System.IO.UnmanagedMemoryStream => 0x5de03f27ab57d1b => 57
	i64 435118502366263740, ; 22: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x609d9f8f8bdb9bc => 298
	i64 449765079935339303, ; 23: StreamJsonRpc.dll => 0x63de2f50debcb27 => 216
	i64 486738060028624626, ; 24: es/Microsoft.ServiceHub.Framework.resources.dll => 0x6c13dafceab3ef2 => 358
	i64 502670939551102150, ; 25: System.Management.dll => 0x6f9d88e66daf4c6 => 229
	i64 545109961164950392, ; 26: fi/Microsoft.Maui.Controls.resources.dll => 0x7909e9f1ec38b78 => 329
	i64 560278790331054453, ; 27: System.Reflection.Primitives => 0x7c6829760de3975 => 96
	i64 602010118039455382, ; 28: zh-Hans\Microsoft.VisualStudio.Utilities.resources => 0x85ac50344eec696 => 420
	i64 634308326490598313, ; 29: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x8cd840fee8b6ba9 => 281
	i64 649145001856603771, ; 30: System.Security.SecureString => 0x90239f09b62167b => 130
	i64 668723562677762733, ; 31: Microsoft.Extensions.Configuration.Binder.dll => 0x947c88986577aad => 183
	i64 675799254782331204, ; 32: ko/Microsoft.VisualStudio.Threading.resources.dll => 0x960ebd778c64944 => 401
	i64 676272570642346617, ; 33: fr/Microsoft.ServiceHub.Resources.dll => 0x9629a51e87a3279 => 372
	i64 702024105029695270, ; 34: System.Drawing.Common => 0x9be17343c0e7726 => 227
	i64 750875890346172408, ; 35: System.Threading.Thread => 0xa6ba5a4da7d1ff8 => 146
	i64 775433664847856186, ; 36: it\Microsoft.VisualStudio.Composition.resources => 0xac2e4cf4c22c23a => 386
	i64 790155713624325618, ; 37: ru\Microsoft.ServiceHub.Framework.resources => 0xaf7326f63d2bdf2 => 365
	i64 798450721097591769, ; 38: Xamarin.AndroidX.Collection.Ktx.dll => 0xb14aab351ad2bd9 => 256
	i64 799765834175365804, ; 39: System.ComponentModel.dll => 0xb1956c9f18442ac => 19
	i64 807768407251849530, ; 40: zh-Hans\Microsoft.VisualStudio.Validation.resources => 0xb35c5162f07913a => 433
	i64 849051935479314978, ; 41: hi/Microsoft.Maui.Controls.resources.dll => 0xbc8703ca21a3a22 => 332
	i64 872800313462103108, ; 42: Xamarin.AndroidX.DrawerLayout => 0xc1ccf42c3c21c44 => 267
	i64 895210737996778430, ; 43: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0xc6c6d6c5569cbbe => 282
	i64 939012430102503855, ; 44: Xamarin.AndroidX.Camera.Video => 0xd080ad47fefa1af => 251
	i64 940822596282819491, ; 45: System.Transactions => 0xd0e792aa81923a3 => 151
	i64 960778385402502048, ; 46: System.Runtime.Handles.dll => 0xd555ed9e1ca1ba0 => 105
	i64 1010599046655515943, ; 47: System.Reflection.Primitives.dll => 0xe065e7a82401d27 => 96
	i64 1120440138749646132, ; 48: Xamarin.Google.Android.Material.dll => 0xf8c9a5eae431534 => 311
	i64 1121665720830085036, ; 49: nb/Microsoft.Maui.Controls.resources.dll => 0xf90f507becf47ac => 340
	i64 1166741338400454851, ; 50: zh-Hant/Microsoft.VisualStudio.Validation.resources.dll => 0x10311910cc1f78c3 => 434
	i64 1182549588684364891, ; 51: zh-Hans\Microsoft.VisualStudio.Threading.resources => 0x10694295e2d0dc5b => 406
	i64 1268860745194512059, ; 52: System.Drawing.dll => 0x119be62002c19ebb => 37
	i64 1301626418029409250, ; 53: System.Diagnostics.FileVersionInfo => 0x12104e54b4e833e2 => 29
	i64 1315114680217950157, ; 54: Xamarin.AndroidX.Arch.Core.Common.dll => 0x124039d5794ad7cd => 245
	i64 1321277010562050883, ; 55: it\CardGameCorner.resources => 0x12561e70d070eb43 => 0
	i64 1369545283391376210, ; 56: Xamarin.AndroidX.Navigation.Fragment.dll => 0x13019a2dd85acb52 => 289
	i64 1404195534211153682, ; 57: System.IO.FileSystem.Watcher.dll => 0x137cb4660bd87f12 => 51
	i64 1425944114962822056, ; 58: System.Runtime.Serialization.dll => 0x13c9f89e19eaf3a8 => 116
	i64 1476839205573959279, ; 59: System.Net.Primitives.dll => 0x147ec96ece9b1e6f => 71
	i64 1486715745332614827, ; 60: Microsoft.Maui.Controls.dll => 0x14a1e017ea87d6ab => 196
	i64 1491290866305144020, ; 61: Xamarin.Google.AutoValue.Annotations.dll => 0x14b2212446e714d4 => 312
	i64 1492954217099365037, ; 62: System.Net.HttpListener => 0x14b809f350210aad => 66
	i64 1513467482682125403, ; 63: Mono.Android.Runtime => 0x1500eaa8245f6c5b => 171
	i64 1534910729791260118, ; 64: zh-Hans/Microsoft.ServiceHub.Framework.resources.dll => 0x154d192d2b24c5d6 => 367
	i64 1537168428375924959, ; 65: System.Threading.Thread.dll => 0x15551e8a954ae0df => 146
	i64 1556147632182429976, ; 66: ko/Microsoft.Maui.Controls.resources.dll => 0x15988c06d24c8918 => 338
	i64 1576750169145655260, ; 67: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x15e1bdecc376bfdc => 310
	i64 1624659445732251991, ; 68: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0x168bf32877da9957 => 244
	i64 1628611045998245443, ; 69: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0x1699fd1e1a00b643 => 285
	i64 1636321030536304333, ; 70: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0x16b5614ec39e16cd => 275
	i64 1651782184287836205, ; 71: System.Globalization.Calendars => 0x16ec4f2524cb982d => 41
	i64 1659332977923810219, ; 72: System.Reflection.DispatchProxy => 0x1707228d493d63ab => 90
	i64 1660779948839220459, ; 73: it/Microsoft.VisualStudio.Threading.resources.dll => 0x170c469074a734eb => 399
	i64 1682513316613008342, ; 74: System.Net.dll => 0x17597cf276952bd6 => 82
	i64 1731380447121279447, ; 75: Newtonsoft.Json => 0x18071957e9b889d7 => 214
	i64 1735388228521408345, ; 76: System.Net.Mail.dll => 0x181556663c69b759 => 67
	i64 1743969030606105336, ; 77: System.Memory.dll => 0x1833d297e88f2af8 => 63
	i64 1767386781656293639, ; 78: System.Private.Uri.dll => 0x188704e9f5582107 => 87
	i64 1795316252682057001, ; 79: Xamarin.AndroidX.AppCompat.dll => 0x18ea3e9eac997529 => 243
	i64 1825687700144851180, ; 80: System.Runtime.InteropServices.RuntimeInformation.dll => 0x1956254a55ef08ec => 107
	i64 1835311033149317475, ; 81: es\Microsoft.Maui.Controls.resources => 0x197855a927386163 => 328
	i64 1836611346387731153, ; 82: Xamarin.AndroidX.SavedState => 0x197cf449ebe482d1 => 296
	i64 1854145951182283680, ; 83: System.Runtime.CompilerServices.VisualC => 0x19bb3feb3df2e3a0 => 103
	i64 1875417405349196092, ; 84: System.Drawing.Primitives => 0x1a06d2319b6c713c => 36
	i64 1875917498431009007, ; 85: Xamarin.AndroidX.Annotation.dll => 0x1a08990699eb70ef => 240
	i64 1881198190668717030, ; 86: tr\Microsoft.Maui.Controls.resources => 0x1a1b5bc992ea9be6 => 350
	i64 1897575647115118287, ; 87: Xamarin.AndroidX.Security.SecurityCrypto => 0x1a558aff4cba86cf => 298
	i64 1911806496241183156, ; 88: tr\Microsoft.VisualStudio.Utilities.resources => 0x1a8819e10fbd69b4 => 419
	i64 1920760634179481754, ; 89: Microsoft.Maui.Controls.Xaml => 0x1aa7e99ec2d2709a => 197
	i64 1930726298510463061, ; 90: CommunityToolkit.Mvvm.dll => 0x1acb5156cd389055 => 177
	i64 1954940598817289193, ; 91: CommunityToolkit.Maui.Camera => 0x1b21581ded7e73e9 => 175
	i64 1959996714666907089, ; 92: tr/Microsoft.Maui.Controls.resources.dll => 0x1b334ea0a2a755d1 => 350
	i64 1972385128188460614, ; 93: System.Security.Cryptography.Algorithms => 0x1b5f51d2edefbe46 => 120
	i64 1981742497975770890, ; 94: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x1b80904d5c241f0a => 283
	i64 1983698669889758782, ; 95: cs/Microsoft.Maui.Controls.resources.dll => 0x1b87836e2031a63e => 324
	i64 2019660174692588140, ; 96: pl/Microsoft.Maui.Controls.resources.dll => 0x1c07463a6f8e1a6c => 342
	i64 2040001226662520565, ; 97: System.Threading.Tasks.Extensions.dll => 0x1c4f8a4ea894a6f5 => 143
	i64 2062890601515140263, ; 98: System.Threading.Tasks.Dataflow => 0x1ca0dc1289cd44a7 => 142
	i64 2080945842184875448, ; 99: System.IO.MemoryMappedFiles => 0x1ce10137d8416db8 => 54
	i64 2102659300918482391, ; 100: System.Drawing.Primitives.dll => 0x1d2e257e6aead5d7 => 36
	i64 2106033277907880740, ; 101: System.Threading.Tasks.Dataflow.dll => 0x1d3a221ba6d9cb24 => 142
	i64 2130792593772371839, ; 102: es/Microsoft.ServiceHub.Resources.dll => 0x1d92189319d5df7f => 371
	i64 2133195048986300728, ; 103: Newtonsoft.Json.dll => 0x1d9aa1984b735138 => 214
	i64 2141794003861218914, ; 104: cs/Microsoft.VisualStudio.Validation.resources.dll => 0x1db92e4c7e35a662 => 422
	i64 2165252314452558154, ; 105: Xamarin.AndroidX.Camera.Camera2.dll => 0x1e0c85820c09814a => 248
	i64 2165310824878145998, ; 106: Xamarin.Android.Glide.GifDecoder => 0x1e0cbab9112b81ce => 237
	i64 2165725771938924357, ; 107: Xamarin.AndroidX.Browser => 0x1e0e341d75540745 => 247
	i64 2200176636225660136, ; 108: Microsoft.Extensions.Logging.Debug.dll => 0x1e8898fe5d5824e8 => 191
	i64 2203565783020068373, ; 109: Xamarin.KotlinX.Coroutines.Core => 0x1e94a367981dde15 => 320
	i64 2262844636196693701, ; 110: Xamarin.AndroidX.DrawerLayout.dll => 0x1f673d352266e6c5 => 267
	i64 2283599909513827695, ; 111: pt-BR/Microsoft.VisualStudio.Validation.resources.dll => 0x1fb0fa04c7af956f => 430
	i64 2287834202362508563, ; 112: System.Collections.Concurrent => 0x1fc00515e8ce7513 => 9
	i64 2287887973817120656, ; 113: System.ComponentModel.DataAnnotations.dll => 0x1fc035fd8d41f790 => 15
	i64 2302323944321350744, ; 114: ru/Microsoft.Maui.Controls.resources.dll => 0x1ff37f6ddb267c58 => 346
	i64 2304837677853103545, ; 115: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0x1ffc6da80d5ed5b9 => 295
	i64 2315304989185124968, ; 116: System.IO.FileSystem.dll => 0x20219d9ee311aa68 => 52
	i64 2329709569556905518, ; 117: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x2054ca829b447e2e => 278
	i64 2335283022205148317, ; 118: ko\Microsoft.VisualStudio.Composition.resources => 0x206897892c89dc9d => 388
	i64 2335503487726329082, ; 119: System.Text.Encodings.Web => 0x2069600c4d9d1cfa => 137
	i64 2337758774805907496, ; 120: System.Runtime.CompilerServices.Unsafe => 0x207163383edbc828 => 102
	i64 2423932764996432979, ; 121: es/Microsoft.VisualStudio.Utilities.resources.dll => 0x21a38a01d90af453 => 411
	i64 2430359805989914396, ; 122: ko\Microsoft.VisualStudio.Utilities.resources => 0x21ba5f5df06a871c => 415
	i64 2470498323731680442, ; 123: Xamarin.AndroidX.CoordinatorLayout => 0x2248f922dc398cba => 260
	i64 2479423007379663237, ; 124: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x2268ae16b2cba985 => 305
	i64 2497223385847772520, ; 125: System.Runtime => 0x22a7eb7046413568 => 117
	i64 2547086958574651984, ; 126: Xamarin.AndroidX.Activity.dll => 0x2359121801df4a50 => 238
	i64 2592350477072141967, ; 127: System.Xml.dll => 0x23f9e10627330e8f => 164
	i64 2602673633151553063, ; 128: th\Microsoft.Maui.Controls.resources => 0x241e8de13a460e27 => 349
	i64 2624866290265602282, ; 129: mscorlib.dll => 0x246d65fbde2db8ea => 167
	i64 2632269733008246987, ; 130: System.Net.NameResolution => 0x2487b36034f808cb => 68
	i64 2656907746661064104, ; 131: Microsoft.Extensions.DependencyInjection => 0x24df3b84c8b75da8 => 184
	i64 2662981627730767622, ; 132: cs\Microsoft.Maui.Controls.resources => 0x24f4cfae6c48af06 => 324
	i64 2704260652175460545, ; 133: System.Composition.Convention => 0x258776bc40fc08c1 => 220
	i64 2706075432581334785, ; 134: System.Net.WebSockets => 0x258de944be6c0701 => 81
	i64 2783046991838674048, ; 135: System.Runtime.CompilerServices.Unsafe.dll => 0x269f5e7e6dc37c80 => 102
	i64 2787234703088983483, ; 136: Xamarin.AndroidX.Startup.StartupRuntime => 0x26ae3f31ef429dbb => 300
	i64 2796533598066548032, ; 137: de\Microsoft.ServiceHub.Resources => 0x26cf487da3339d40 => 370
	i64 2815524396660695947, ; 138: System.Security.AccessControl => 0x2712c0857f68238b => 118
	i64 2844780895111324988, ; 139: System.Composition.TypedParts => 0x277ab126dceda53c => 223
	i64 2851879596360956261, ; 140: System.Configuration.ConfigurationManager => 0x2793e9620b477965 => 224
	i64 2874659109084101369, ; 141: de/Microsoft.VisualStudio.Validation.resources.dll => 0x27e4d73aa74c7af9 => 423
	i64 2895129759130297543, ; 142: fi\Microsoft.Maui.Controls.resources => 0x282d912d479fa4c7 => 329
	i64 2923871038697555247, ; 143: Jsr305Binding => 0x2893ad37e69ec52f => 313
	i64 2957510711280508021, ; 144: zh-Hant/Microsoft.VisualStudio.Threading.resources.dll => 0x290b305285499c75 => 407
	i64 2991907748835029057, ; 145: de/StreamJsonRpc.resources.dll => 0x2985643eea0cf041 => 436
	i64 3017136373564924869, ; 146: System.Net.WebProxy => 0x29df058bd93f63c5 => 79
	i64 3017704767998173186, ; 147: Xamarin.Google.Android.Material => 0x29e10a7f7d88a002 => 311
	i64 3106852385031680087, ; 148: System.Runtime.Serialization.Xml => 0x2b1dc1c88b637057 => 115
	i64 3110390492489056344, ; 149: System.Security.Cryptography.Csp.dll => 0x2b2a53ac61900058 => 122
	i64 3135773902340015556, ; 150: System.IO.FileSystem.DriveInfo.dll => 0x2b8481c008eac5c4 => 49
	i64 3216235690883836477, ; 151: cs/Microsoft.VisualStudio.Utilities.resources.dll => 0x2ca25d520d106a3d => 408
	i64 3238539556702659506, ; 152: Microsoft.Win32.SystemEvents.dll => 0x2cf19a917c5023b2 => 212
	i64 3281594302220646930, ; 153: System.Security.Principal => 0x2d8a90a198ceba12 => 129
	i64 3289520064315143713, ; 154: Xamarin.AndroidX.Lifecycle.Common => 0x2da6b911e3063621 => 276
	i64 3303437397778967116, ; 155: Xamarin.AndroidX.Annotation.Experimental => 0x2dd82acf985b2a4c => 241
	i64 3311221304742556517, ; 156: System.Numerics.Vectors.dll => 0x2df3d23ba9e2b365 => 83
	i64 3325875462027654285, ; 157: System.Runtime.Numerics => 0x2e27e21c8958b48d => 111
	i64 3328853167529574890, ; 158: System.Net.Sockets.dll => 0x2e327651a008c1ea => 76
	i64 3341463492655001636, ; 159: it\Microsoft.ServiceHub.Framework.resources => 0x2e5f4357bda33024 => 360
	i64 3344514922410554693, ; 160: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x2e6a1a9a18463545 => 321
	i64 3373397458776442943, ; 161: tr/Microsoft.ServiceHub.Resources.dll => 0x2ed0b71da093483f => 379
	i64 3378246407733840341, ; 162: pl\Microsoft.VisualStudio.Threading.resources => 0x2ee1f13588b709d5 => 402
	i64 3429672777697402584, ; 163: Microsoft.Maui.Essentials => 0x2f98a5385a7b1ed8 => 199
	i64 3437845325506641314, ; 164: System.IO.MemoryMappedFiles.dll => 0x2fb5ae1beb8f7da2 => 54
	i64 3493805808809882663, ; 165: Xamarin.AndroidX.Tracing.Tracing.dll => 0x307c7ddf444f3427 => 302
	i64 3494946837667399002, ; 166: Microsoft.Extensions.Configuration => 0x30808ba1c00a455a => 181
	i64 3508450208084372758, ; 167: System.Net.Ping => 0x30b084e02d03ad16 => 70
	i64 3522470458906976663, ; 168: Xamarin.AndroidX.SwipeRefreshLayout => 0x30e2543832f52197 => 301
	i64 3531994851595924923, ; 169: System.Numerics => 0x31042a9aade235bb => 84
	i64 3532505434160716017, ; 170: pt-BR\Microsoft.ServiceHub.Framework.resources => 0x3105faf9f24958f1 => 364
	i64 3551103847008531295, ; 171: System.Private.CoreLib.dll => 0x31480e226177735f => 173
	i64 3567343442040498961, ; 172: pt\Microsoft.Maui.Controls.resources => 0x3181bff5bea4ab11 => 344
	i64 3571415421602489686, ; 173: System.Runtime.dll => 0x319037675df7e556 => 117
	i64 3638003163729360188, ; 174: Microsoft.Extensions.Configuration.Abstractions => 0x327cc89a39d5f53c => 182
	i64 3647754201059316852, ; 175: System.Xml.ReaderWriter => 0x329f6d1e86145474 => 157
	i64 3655542548057982301, ; 176: Microsoft.Extensions.Configuration.dll => 0x32bb18945e52855d => 181
	i64 3659371656528649588, ; 177: Xamarin.Android.Glide.Annotations => 0x32c8b3222885dd74 => 235
	i64 3690127606734143618, ; 178: fr/Microsoft.VisualStudio.Validation.resources.dll => 0x3335f781d7404082 => 425
	i64 3716579019761409177, ; 179: netstandard.dll => 0x3393f0ed5c8c5c99 => 168
	i64 3727469159507183293, ; 180: Xamarin.AndroidX.RecyclerView => 0x33baa1739ba646bd => 294
	i64 3745789886814214634, ; 181: ko\Microsoft.ServiceHub.Framework.resources => 0x33fbb80e56cfd9ea => 362
	i64 3772598417116884899, ; 182: Xamarin.AndroidX.DynamicAnimation.dll => 0x345af645b473efa3 => 268
	i64 3774851544379141081, ; 183: it\Microsoft.ServiceHub.Resources => 0x3462f77ac68dabd9 => 373
	i64 3808596350265393157, ; 184: System.Diagnostics.PerformanceCounter.dll => 0x34dada33a66b0005 => 226
	i64 3829576749922459295, ; 185: cs/Microsoft.ServiceHub.Framework.resources.dll => 0x352563c39b391e9f => 356
	i64 3869221888984012293, ; 186: Microsoft.Extensions.Logging.dll => 0x35b23cceda0ed605 => 189
	i64 3869649043256705283, ; 187: System.Diagnostics.Tools => 0x35b3c14d74bf0103 => 33
	i64 3875180953283865480, ; 188: MessagePack.dll => 0x35c7688ba0d82b88 => 179
	i64 3890352374528606784, ; 189: Microsoft.Maui.Controls.Xaml.dll => 0x35fd4edf66e00240 => 197
	i64 3892323582453208937, ; 190: ko/Microsoft.VisualStudio.Utilities.resources.dll => 0x36044fad02ffd769 => 415
	i64 3919223565570527920, ; 191: System.Security.Cryptography.Encoding => 0x3663e111652bd2b0 => 123
	i64 3933965368022646939, ; 192: System.Net.Requests => 0x369840a8bfadc09b => 73
	i64 3966267475168208030, ; 193: System.Memory => 0x370b03412596249e => 63
	i64 3986466921713458903, ; 194: System.Composition.Hosting => 0x3752c68b49935ed7 => 221
	i64 4006972109285359177, ; 195: System.Xml.XmlDocument => 0x379b9fe74ed9fe49 => 162
	i64 4009997192427317104, ; 196: System.Runtime.Serialization.Primitives => 0x37a65f335cf1a770 => 114
	i64 4020380517496724220, ; 197: MessagePack.Annotations.dll => 0x37cb42c79f4b1afc => 180
	i64 4073500526318903918, ; 198: System.Private.Xml.dll => 0x3887fb25779ae26e => 89
	i64 4073631083018132676, ; 199: Microsoft.Maui.Controls.Compatibility.dll => 0x388871e311491cc4 => 195
	i64 4120493066591692148, ; 200: zh-Hant\Microsoft.Maui.Controls.resources => 0x392eee9cdda86574 => 355
	i64 4148881117810174540, ; 201: System.Runtime.InteropServices.JavaScript.dll => 0x3993c9651a66aa4c => 106
	i64 4154383907710350974, ; 202: System.ComponentModel => 0x39a7562737acb67e => 19
	i64 4167269041631776580, ; 203: System.Threading.ThreadPool => 0x39d51d1d3df1cf44 => 147
	i64 4168469861834746866, ; 204: System.Security.Claims.dll => 0x39d96140fb94ebf2 => 119
	i64 4187479170553454871, ; 205: System.Linq.Expressions => 0x3a1cea1e912fa117 => 59
	i64 4201423742386704971, ; 206: Xamarin.AndroidX.Core.Core.Ktx => 0x3a4e74a233da124b => 262
	i64 4205801962323029395, ; 207: System.ComponentModel.TypeConverter => 0x3a5e0299f7e7ad93 => 18
	i64 4224288942162558427, ; 208: Microsoft.VisualStudio.Composition.dll => 0x3a9fb0696235bddb => 204
	i64 4235503420553921860, ; 209: System.IO.IsolatedStorage.dll => 0x3ac787eb9b118544 => 53
	i64 4237761919127609754, ; 210: tr\Microsoft.ServiceHub.Resources => 0x3acf8e034847619a => 379
	i64 4282138915307457788, ; 211: System.Reflection.Emit => 0x3b6d36a7ddc70cfc => 93
	i64 4321865999928413850, ; 212: System.Diagnostics.EventLog.dll => 0x3bfa5a3a8c924e9a => 225
	i64 4356591372459378815, ; 213: vi/Microsoft.Maui.Controls.resources.dll => 0x3c75b8c562f9087f => 352
	i64 4373617458794931033, ; 214: System.IO.Pipes.dll => 0x3cb235e806eb2359 => 56
	i64 4397634830160618470, ; 215: System.Security.SecureString.dll => 0x3d0789940f9be3e6 => 130
	i64 4462330757387019284, ; 216: Microsoft.ServiceHub.Resources => 0x3ded622e705a6414 => 203
	i64 4477672992252076438, ; 217: System.Web.HttpUtility.dll => 0x3e23e3dcdb8ba196 => 153
	i64 4484706122338676047, ; 218: System.Globalization.Extensions.dll => 0x3e3ce07510042d4f => 42
	i64 4533124835995628778, ; 219: System.Reflection.Emit.dll => 0x3ee8e505540534ea => 93
	i64 4537536889469555869, ; 220: es/Microsoft.VisualStudio.Composition.resources.dll => 0x3ef891c29410549d => 384
	i64 4636684751163556186, ; 221: Xamarin.AndroidX.VersionedParcelable.dll => 0x4058d0370893015a => 306
	i64 4654490921503260514, ; 222: fr\Microsoft.VisualStudio.Validation.resources => 0x409812d5227b7f62 => 425
	i64 4657212095206026001, ; 223: Microsoft.Extensions.Http.dll => 0x40a1bdb9c2686b11 => 188
	i64 4672453897036726049, ; 224: System.IO.FileSystem.Watcher => 0x40d7e4104a437f21 => 51
	i64 4679594760078841447, ; 225: ar/Microsoft.Maui.Controls.resources.dll => 0x40f142a407475667 => 322
	i64 4692118866197340027, ; 226: pl/Microsoft.VisualStudio.Composition.resources.dll => 0x411dc13fb799df7b => 389
	i64 4716677666592453464, ; 227: System.Xml.XmlSerializer => 0x417501590542f358 => 163
	i64 4723252672199750351, ; 228: zh-Hant\StreamJsonRpc.resources => 0x418c5d47cee172cf => 447
	i64 4725285941539738176, ; 229: Xamarin.AndroidX.Camera.Lifecycle => 0x41939687379f9240 => 250
	i64 4743821336939966868, ; 230: System.ComponentModel.Annotations => 0x41d5705f4239b194 => 14
	i64 4759461199762736555, ; 231: Xamarin.AndroidX.Lifecycle.Process.dll => 0x420d00be961cc5ab => 280
	i64 4794310189461587505, ; 232: Xamarin.AndroidX.Activity => 0x4288cfb749e4c631 => 238
	i64 4795410492532947900, ; 233: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0x428cb86f8f9b7bbc => 301
	i64 4809057822547766521, ; 234: System.Drawing => 0x42bd349c3145ecf9 => 37
	i64 4814660307502931973, ; 235: System.Net.NameResolution.dll => 0x42d11c0a5ee2a005 => 68
	i64 4853321196694829351, ; 236: System.Runtime.Loader.dll => 0x435a75ea15de7927 => 110
	i64 4952488434027013261, ; 237: pl/Microsoft.VisualStudio.Validation.resources.dll => 0x44bac5fdc869248d => 429
	i64 4966213257581017890, ; 238: it\Microsoft.VisualStudio.Threading.resources => 0x44eb88a548f3bb22 => 399
	i64 5002337827157170690, ; 239: zh-Hant\Microsoft.ServiceHub.Resources => 0x456bdfc01233da02 => 381
	i64 5051607678629112937, ; 240: es\Microsoft.ServiceHub.Framework.resources => 0x461aea6b4d52ec69 => 358
	i64 5055365687667823624, ; 241: Xamarin.AndroidX.Activity.Ktx.dll => 0x4628444ef7239408 => 239
	i64 5072195939344590231, ; 242: cs/Microsoft.VisualStudio.Threading.resources.dll => 0x46640f554bd02597 => 395
	i64 5081566143765835342, ; 243: System.Resources.ResourceManager.dll => 0x4685597c05d06e4e => 100
	i64 5099468265966638712, ; 244: System.Resources.ResourceManager => 0x46c4f35ea8519678 => 100
	i64 5103417709280584325, ; 245: System.Collections.Specialized => 0x46d2fb5e161b6285 => 12
	i64 5182934613077526976, ; 246: System.Collections.Specialized.dll => 0x47ed7b91fa9009c0 => 12
	i64 5205316157927637098, ; 247: Xamarin.AndroidX.LocalBroadcastManager => 0x483cff7778e0c06a => 287
	i64 5244375036463807528, ; 248: System.Diagnostics.Contracts.dll => 0x48c7c34f4d59fc28 => 26
	i64 5262971552273843408, ; 249: System.Security.Principal.dll => 0x4909d4be0c44c4d0 => 129
	i64 5278787618751394462, ; 250: System.Net.WebClient.dll => 0x4942055efc68329e => 77
	i64 5280980186044710147, ; 251: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x4949cf7fd7123d03 => 279
	i64 5287793306032704698, ; 252: zh-Hans/Microsoft.ServiceHub.Resources.dll => 0x496203fef3c48cba => 380
	i64 5290786973231294105, ; 253: System.Runtime.Loader => 0x496ca6b869b72699 => 110
	i64 5313674019156125223, ; 254: de\Microsoft.ServiceHub.Framework.resources => 0x49bdf65e0c05e627 => 357
	i64 5340835533783596017, ; 255: cs/Microsoft.ServiceHub.Resources.dll => 0x4a1e759efaf47bf1 => 369
	i64 5376510917114486089, ; 256: Xamarin.AndroidX.VectorDrawable.Animated => 0x4a9d3431719e5d49 => 305
	i64 5408338804355907810, ; 257: Xamarin.AndroidX.Transition => 0x4b0e477cea9840e2 => 303
	i64 5423376490970181369, ; 258: System.Runtime.InteropServices.RuntimeInformation => 0x4b43b42f2b7b6ef9 => 107
	i64 5424449234786381625, ; 259: cs\Microsoft.VisualStudio.Validation.resources => 0x4b4783d6cd94ef39 => 422
	i64 5435342863934572396, ; 260: ru/Microsoft.VisualStudio.Utilities.resources.dll => 0x4b6e37897d46476c => 418
	i64 5440320908473006344, ; 261: Microsoft.VisualBasic.Core => 0x4b7fe70acda9f908 => 3
	i64 5446034149219586269, ; 262: System.Diagnostics.Debug => 0x4b94333452e150dd => 27
	i64 5451019430259338467, ; 263: Xamarin.AndroidX.ConstraintLayout.dll => 0x4ba5e94a845c2ce3 => 258
	i64 5457765010617926378, ; 264: System.Xml.Serialization => 0x4bbde05c557002ea => 158
	i64 5471532531798518949, ; 265: sv\Microsoft.Maui.Controls.resources => 0x4beec9d926d82ca5 => 348
	i64 5488847537322884930, ; 266: System.Windows.Extensions => 0x4c2c4dc108687f42 => 233
	i64 5499883080459888738, ; 267: Microsoft.VisualStudio.Threading => 0x4c538285a4fe2862 => 208
	i64 5507995362134886206, ; 268: System.Core.dll => 0x4c705499688c873e => 22
	i64 5522859530602327440, ; 269: uk\Microsoft.Maui.Controls.resources => 0x4ca5237b51eead90 => 351
	i64 5527431512186326818, ; 270: System.IO.FileSystem.Primitives.dll => 0x4cb561acbc2a8f22 => 50
	i64 5570799893513421663, ; 271: System.IO.Compression.Brotli => 0x4d4f74fcdfa6c35f => 44
	i64 5573260873512690141, ; 272: System.Security.Cryptography.dll => 0x4d58333c6e4ea1dd => 127
	i64 5574231584441077149, ; 273: Xamarin.AndroidX.Annotation.Jvm => 0x4d5ba617ae5f8d9d => 242
	i64 5591791169662171124, ; 274: System.Linq.Parallel => 0x4d9a087135e137f4 => 60
	i64 5635158070688529302, ; 275: es/Microsoft.VisualStudio.Validation.resources.dll => 0x4e341a68b090bb96 => 424
	i64 5650097808083101034, ; 276: System.Security.Cryptography.Algorithms.dll => 0x4e692e055d01a56a => 120
	i64 5676263520774316728, ; 277: Microsoft.VisualStudio.Validation => 0x4ec623991742e2b8 => 211
	i64 5681707193364847894, ; 278: pt-BR/StreamJsonRpc.resources.dll => 0x4ed97a96e24b1d16 => 443
	i64 5692067934154308417, ; 279: Xamarin.AndroidX.ViewPager2.dll => 0x4efe49a0d4a8bb41 => 308
	i64 5724799082821825042, ; 280: Xamarin.AndroidX.ExifInterface => 0x4f72926f3e13b212 => 271
	i64 5757522595884336624, ; 281: Xamarin.AndroidX.Concurrent.Futures.dll => 0x4fe6d44bd9f885f0 => 257
	i64 5783556987928984683, ; 282: Microsoft.VisualBasic => 0x504352701bbc3c6b => 4
	i64 5815588864042668787, ; 283: de\StreamJsonRpc.resources => 0x50b51f4270fb12f3 => 436
	i64 5896680224035167651, ; 284: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x51d5376bfbafdda3 => 277
	i64 5924126145606300664, ; 285: pl\StreamJsonRpc.resources => 0x5236b9579177b3f8 => 442
	i64 5959344983920014087, ; 286: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x52b3d8b05c8ef307 => 297
	i64 5979151488806146654, ; 287: System.Formats.Asn1 => 0x52fa3699a489d25e => 39
	i64 5984759512290286505, ; 288: System.Security.Cryptography.Primitives => 0x530e23115c33dba9 => 125
	i64 6010974535988770325, ; 289: Microsoft.Extensions.Diagnostics.dll => 0x536b457e33877615 => 186
	i64 6068057819846744445, ; 290: ro/Microsoft.Maui.Controls.resources.dll => 0x5436126fec7f197d => 345
	i64 6102788177522843259, ; 291: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0x54b1758374b3de7b => 297
	i64 6200764641006662125, ; 292: ro\Microsoft.Maui.Controls.resources => 0x560d8a96830131ed => 345
	i64 6222399776351216807, ; 293: System.Text.Json.dll => 0x565a67a0ffe264a7 => 138
	i64 6251069312384999852, ; 294: System.Transactions.Local => 0x56c0426b870da1ac => 150
	i64 6278736998281604212, ; 295: System.Private.DataContractSerialization => 0x57228e08a4ad6c74 => 86
	i64 6284145129771520194, ; 296: System.Reflection.Emit.ILGeneration => 0x5735c4b3610850c2 => 91
	i64 6315590403487358180, ; 297: fr\StreamJsonRpc.resources => 0x57a57c02b2badce4 => 438
	i64 6319713645133255417, ; 298: Xamarin.AndroidX.Lifecycle.Runtime => 0x57b42213b45b52f9 => 281
	i64 6355862529564748107, ; 299: Microsoft.ServiceHub.Resources.dll => 0x58348f4bbbec294b => 203
	i64 6357457916754632952, ; 300: _Microsoft.Android.Resource.Designer => 0x583a3a4ac2a7a0f8 => 448
	i64 6401687960814735282, ; 301: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0x58d75d486341cfb2 => 278
	i64 6435971861494892919, ; 302: cs/StreamJsonRpc.resources.dll => 0x59512a4f0cd00977 => 435
	i64 6474126617406440626, ; 303: Microsoft.VisualStudio.RpcContracts.dll => 0x59d8b7db6d6984b2 => 206
	i64 6478287442656530074, ; 304: hr\Microsoft.Maui.Controls.resources => 0x59e7801b0c6a8e9a => 333
	i64 6504860066809920875, ; 305: Xamarin.AndroidX.Browser.dll => 0x5a45e7c43bd43d6b => 247
	i64 6548213210057960872, ; 306: Xamarin.AndroidX.CustomView.dll => 0x5adfed387b066da8 => 264
	i64 6557084851308642443, ; 307: Xamarin.AndroidX.Window.dll => 0x5aff71ee6c58c08b => 309
	i64 6560151584539558821, ; 308: Microsoft.Extensions.Options => 0x5b0a571be53243a5 => 192
	i64 6589202984700901502, ; 309: Xamarin.Google.ErrorProne.Annotations.dll => 0x5b718d34180a787e => 315
	i64 6591971792923354531, ; 310: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x5b7b636b7e9765a3 => 279
	i64 6617685658146568858, ; 311: System.Text.Encoding.CodePages => 0x5bd6be0b4905fa9a => 134
	i64 6671798237668743565, ; 312: SkiaSharp => 0x5c96fd260152998d => 215
	i64 6692579329111691486, ; 313: it/Microsoft.VisualStudio.Composition.resources.dll => 0x5ce0d170e6d310de => 386
	i64 6713049226152416138, ; 314: cs\Microsoft.VisualStudio.Threading.resources => 0x5d298ab43486938a => 395
	i64 6713440830605852118, ; 315: System.Reflection.TypeExtensions.dll => 0x5d2aeeddb8dd7dd6 => 97
	i64 6739246970287767386, ; 316: tr/StreamJsonRpc.resources.dll => 0x5d869d69d5a13f5a => 445
	i64 6739853162153639747, ; 317: Microsoft.VisualBasic.dll => 0x5d88c4bde075ff43 => 4
	i64 6743165466166707109, ; 318: nl\Microsoft.Maui.Controls.resources => 0x5d948943c08c43a5 => 341
	i64 6746589141607518456, ; 319: pt-BR/Microsoft.VisualStudio.Utilities.resources.dll => 0x5da0b3144f0610f8 => 417
	i64 6766076348542393491, ; 320: Microsoft.ServiceHub.Framework.dll => 0x5de5ee973e0a9c93 => 202
	i64 6772837112740759457, ; 321: System.Runtime.InteropServices.JavaScript => 0x5dfdf378527ec7a1 => 106
	i64 6777482997383978746, ; 322: pt/Microsoft.Maui.Controls.resources.dll => 0x5e0e74e0a2525efa => 344
	i64 6786606130239981554, ; 323: System.Diagnostics.TraceSource => 0x5e2ede51877147f2 => 34
	i64 6791396597476077909, ; 324: de/Microsoft.VisualStudio.Utilities.resources.dll => 0x5e3fe339195bb155 => 409
	i64 6798329586179154312, ; 325: System.Windows => 0x5e5884bd523ca188 => 155
	i64 6814185388980153342, ; 326: System.Xml.XDocument.dll => 0x5e90d98217d1abfe => 159
	i64 6876862101832370452, ; 327: System.Xml.Linq => 0x5f6f85a57d108914 => 156
	i64 6894844156784520562, ; 328: System.Numerics.Vectors => 0x5faf683aead1ad72 => 83
	i64 6987056692196838363, ; 329: System.Management => 0x60f7030ae3e88bdb => 229
	i64 7011053663211085209, ; 330: Xamarin.AndroidX.Fragment.Ktx => 0x614c442918e5dd99 => 273
	i64 7014127154236225539, ; 331: ru\Microsoft.ServiceHub.Resources => 0x61572f7bfdabf803 => 378
	i64 7060896174307865760, ; 332: System.Threading.Tasks.Parallel.dll => 0x61fd57a90988f4a0 => 144
	i64 7083547580668757502, ; 333: System.Private.Xml.Linq.dll => 0x624dd0fe8f56c5fe => 88
	i64 7101497697220435230, ; 334: System.Configuration => 0x628d9687c0141d1e => 20
	i64 7103753931438454322, ; 335: Xamarin.AndroidX.Interpolator.dll => 0x62959a90372c7632 => 274
	i64 7112547816752919026, ; 336: System.IO.FileSystem => 0x62b4d88e3189b1f2 => 52
	i64 7118095232192617785, ; 337: tr\Microsoft.VisualStudio.Composition.resources => 0x62c88de6803e1d39 => 392
	i64 7188876148444261747, ; 338: System.Composition.AttributedModel.dll => 0x63c404c4ca4c6d73 => 219
	i64 7192745174564810625, ; 339: Xamarin.Android.Glide.GifDecoder.dll => 0x63d1c3a0a1d72f81 => 237
	i64 7220009545223068405, ; 340: sv/Microsoft.Maui.Controls.resources.dll => 0x6432a06d99f35af5 => 348
	i64 7270811800166795866, ; 341: System.Linq => 0x64e71ccf51a90a5a => 62
	i64 7284491514466782135, ; 342: pt-BR\Microsoft.ServiceHub.Resources => 0x6517b6700123cbb7 => 377
	i64 7299370801165188114, ; 343: System.IO.Pipes.AccessControl.dll => 0x654c9311e74f3c12 => 55
	i64 7316205155833392065, ; 344: Microsoft.Win32.Primitives => 0x658861d38954abc1 => 5
	i64 7338192458477945005, ; 345: System.Reflection => 0x65d67f295d0740ad => 98
	i64 7349431895026339542, ; 346: Xamarin.Android.Glide.DiskLruCache => 0x65fe6d5e9bf88ed6 => 236
	i64 7371656174704040109, ; 347: ru\Microsoft.VisualStudio.Composition.resources => 0x664d623bf38c98ad => 391
	i64 7377312882064240630, ; 348: System.ComponentModel.TypeConverter.dll => 0x66617afac45a2ff6 => 18
	i64 7488575175965059935, ; 349: System.Xml.Linq.dll => 0x67ecc3724534ab5f => 156
	i64 7489048572193775167, ; 350: System.ObjectModel => 0x67ee71ff6b419e3f => 85
	i64 7584834483252033044, ; 351: pt-BR\Microsoft.VisualStudio.Validation.resources => 0x6942bec6be5ef614 => 430
	i64 7592577537120840276, ; 352: System.Diagnostics.Process => 0x695e410af5b2aa54 => 30
	i64 7635299587268401828, ; 353: ja/Microsoft.ServiceHub.Framework.resources.dll => 0x69f6088564d1daa4 => 361
	i64 7637303409920963731, ; 354: System.IO.Compression.ZipFile.dll => 0x69fd26fcb637f493 => 46
	i64 7643889328271876492, ; 355: fr\Microsoft.VisualStudio.Composition.resources => 0x6a148cd8520b618c => 385
	i64 7654504624184590948, ; 356: System.Net.Http => 0x6a3a4366801b8264 => 65
	i64 7683996460257074599, ; 357: it\Microsoft.VisualStudio.Validation.resources => 0x6aa30a11acfb5da7 => 426
	i64 7692844685477678523, ; 358: de\Microsoft.VisualStudio.Composition.resources => 0x6ac2797ba6260dbb => 383
	i64 7694700312542370399, ; 359: System.Net.Mail => 0x6ac9112a7e2cda5f => 67
	i64 7708790323521193081, ; 360: ms/Microsoft.Maui.Controls.resources.dll => 0x6afb1ff4d1730479 => 339
	i64 7714652370974252055, ; 361: System.Private.CoreLib => 0x6b0ff375198b9c17 => 173
	i64 7725404731275645577, ; 362: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x6b3626ac11ce9289 => 282
	i64 7735176074855944702, ; 363: Microsoft.CSharp => 0x6b58dda848e391fe => 2
	i64 7735352534559001595, ; 364: Xamarin.Kotlin.StdLib.dll => 0x6b597e2582ce8bfb => 318
	i64 7756332380610150586, ; 365: Xamarin.Google.AutoValue.Annotations => 0x6ba407349220c0ba => 312
	i64 7791074099216502080, ; 366: System.IO.FileSystem.AccessControl.dll => 0x6c1f749d468bcd40 => 48
	i64 7820441508502274321, ; 367: System.Data => 0x6c87ca1e14ff8111 => 25
	i64 7836164640616011524, ; 368: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x6cbfa6390d64d704 => 244
	i64 7889145416849571422, ; 369: fr/Microsoft.VisualStudio.Composition.resources.dll => 0x6d7bdff548b83e5e => 385
	i64 7919757340696389605, ; 370: Microsoft.Extensions.Diagnostics.Abstractions => 0x6de8a157378027e5 => 187
	i64 7921741049276291146, ; 371: ja/Microsoft.VisualStudio.Threading.resources.dll => 0x6defad835cbe584a => 400
	i64 8020916254412992903, ; 372: zh-Hant/Microsoft.ServiceHub.Resources.dll => 0x6f5004d635717187 => 381
	i64 8025517457475554965, ; 373: WindowsBase => 0x6f605d9b4786ce95 => 166
	i64 8031450141206250471, ; 374: System.Runtime.Intrinsics.dll => 0x6f757159d9dc03e7 => 109
	i64 8037258329461389554, ; 375: ru/StreamJsonRpc.resources.dll => 0x6f8a13de0f9878f2 => 444
	i64 8044118961405839122, ; 376: System.ComponentModel.Composition => 0x6fa2739369944712 => 218
	i64 8064050204834738623, ; 377: System.Collections.dll => 0x6fe942efa61731bf => 13
	i64 8083354569033831015, ; 378: Xamarin.AndroidX.Lifecycle.Common.dll => 0x702dd82730cad267 => 276
	i64 8085230611270010360, ; 379: System.Net.Http.Json.dll => 0x703482674fdd05f8 => 64
	i64 8087206902342787202, ; 380: System.Diagnostics.DiagnosticSource => 0x703b87d46f3aa082 => 28
	i64 8103644804370223335, ; 381: System.Data.DataSetExtensions.dll => 0x7075ee03be6d50e7 => 24
	i64 8113615946733131500, ; 382: System.Reflection.Extensions => 0x70995ab73cf916ec => 94
	i64 8167236081217502503, ; 383: Java.Interop.dll => 0x7157d9f1a9b8fd27 => 169
	i64 8185542183669246576, ; 384: System.Collections => 0x7198e33f4794aa70 => 13
	i64 8187640529827139739, ; 385: Xamarin.KotlinX.Coroutines.Android => 0x71a057ae90f0109b => 319
	i64 8199901720993286004, ; 386: pl/StreamJsonRpc.resources.dll => 0x71cbe72b98eb4774 => 442
	i64 8202143405964630621, ; 387: de\Microsoft.VisualStudio.Threading.resources => 0x71d3ddf88d559a5d => 396
	i64 8234853665224889887, ; 388: it/CardGameCorner.resources.dll => 0x724813c7450d961f => 0
	i64 8246048515196606205, ; 389: Microsoft.Maui.Graphics.dll => 0x726fd96f64ee56fd => 200
	i64 8264926008854159966, ; 390: System.Diagnostics.Process.dll => 0x72b2ea6a64a3a25e => 30
	i64 8290740647658429042, ; 391: System.Runtime.Extensions => 0x730ea0b15c929a72 => 104
	i64 8311982169281665208, ; 392: System.Threading.AccessControl => 0x735a17be836a18b8 => 232
	i64 8318905602908530212, ; 393: System.ComponentModel.DataAnnotations => 0x7372b092055ea624 => 15
	i64 8320777595162576093, ; 394: Xamarin.AndroidX.Camera.View => 0x737957232eb1c4dd => 252
	i64 8368701292315763008, ; 395: System.Security.Cryptography => 0x7423997c6fd56140 => 127
	i64 8398329775253868912, ; 396: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x748cdc6f3097d170 => 259
	i64 8400357532724379117, ; 397: Xamarin.AndroidX.Navigation.UI.dll => 0x749410ab44503ded => 291
	i64 8406449386989781371, ; 398: pt-BR/Microsoft.VisualStudio.Composition.resources.dll => 0x74a9b52d9dd3817b => 390
	i64 8410671156615598628, ; 399: System.Reflection.Emit.Lightweight.dll => 0x74b8b4daf4b25224 => 92
	i64 8426919725312979251, ; 400: Xamarin.AndroidX.Lifecycle.Process => 0x74f26ed7aa033133 => 280
	i64 8445070827284110693, ; 401: Microsoft.VisualStudio.Utilities.Internal => 0x7532eb2c6b489d65 => 210
	i64 8476857680833348370, ; 402: System.Security.Permissions.dll => 0x75a3d925fd9d0312 => 231
	i64 8518412311883997971, ; 403: System.Collections.Immutable => 0x76377add7c28e313 => 10
	i64 8540594166003620310, ; 404: ko/Microsoft.ServiceHub.Framework.resources.dll => 0x76864924db6b59d6 => 362
	i64 8563666267364444763, ; 405: System.Private.Uri => 0x76d841191140ca5b => 87
	i64 8568254372587209012, ; 406: it/Microsoft.VisualStudio.Utilities.resources.dll => 0x76e88df48f3f8134 => 413
	i64 8575739357155667659, ; 407: zh-Hant\Microsoft.VisualStudio.Utilities.resources => 0x770325825599aecb => 421
	i64 8595034769595574800, ; 408: ja\Microsoft.VisualStudio.Threading.resources => 0x7747b295a1e02a10 => 400
	i64 8598790081731763592, ; 409: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x77550a055fc61d88 => 270
	i64 8599632406834268464, ; 410: CommunityToolkit.Maui => 0x7758081c784b4930 => 174
	i64 8601935802264776013, ; 411: Xamarin.AndroidX.Transition.dll => 0x7760370982b4ed4d => 303
	i64 8605570381828407308, ; 412: en/Microsoft.VisualStudio.Utilities.resources.dll => 0x776d20ab0281400c => 410
	i64 8614108721271900878, ; 413: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x778b763e14018ace => 343
	i64 8623059219396073920, ; 414: System.Net.Quic.dll => 0x77ab42ac514299c0 => 72
	i64 8626175481042262068, ; 415: Java.Interop => 0x77b654e585b55834 => 169
	i64 8629545377263870989, ; 416: Xamarin.AndroidX.Camera.Core.dll => 0x77c24dcca0ed640d => 249
	i64 8638972117149407195, ; 417: Microsoft.CSharp.dll => 0x77e3cb5e8b31d7db => 2
	i64 8639588376636138208, ; 418: Xamarin.AndroidX.Navigation.Runtime => 0x77e5fbdaa2fda2e0 => 290
	i64 8648495978913578441, ; 419: Microsoft.Win32.Registry.dll => 0x7805a1456889bdc9 => 6
	i64 8677882282824630478, ; 420: pt-BR\Microsoft.Maui.Controls.resources => 0x786e07f5766b00ce => 343
	i64 8684531736582871431, ; 421: System.IO.Compression.FileSystem => 0x7885a79a0fa0d987 => 45
	i64 8725526185868997716, ; 422: System.Diagnostics.DiagnosticSource.dll => 0x79174bd613173454 => 28
	i64 8740830839790451704, ; 423: fr\Microsoft.ServiceHub.Framework.resources => 0x794dab567f7d3bf8 => 359
	i64 8772037756965498393, ; 424: it/Microsoft.VisualStudio.Validation.resources.dll => 0x79bc89dd1c3e2e19 => 426
	i64 8816904670177563593, ; 425: Microsoft.Extensions.Diagnostics => 0x7a5bf015646a93c9 => 186
	i64 8825974560710680624, ; 426: pl/Microsoft.ServiceHub.Framework.resources.dll => 0x7a7c2919d7cb6030 => 363
	i64 8907357455289697290, ; 427: es/Microsoft.VisualStudio.Threading.resources.dll => 0x7b9d4a6991f8440a => 397
	i64 8941376889969657626, ; 428: System.Xml.XDocument => 0x7c1626e87187471a => 159
	i64 8951477988056063522, ; 429: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0x7c3a09cd9ccf5e22 => 293
	i64 8954753533646919997, ; 430: System.Runtime.Serialization.Json => 0x7c45ace50032d93d => 113
	i64 8993496018399434829, ; 431: de/Microsoft.ServiceHub.Framework.resources.dll => 0x7ccf50faa996984d => 357
	i64 9031035476476434958, ; 432: Xamarin.KotlinX.Coroutines.Core.dll => 0x7d54aeead9541a0e => 320
	i64 9045785047181495996, ; 433: zh-HK\Microsoft.Maui.Controls.resources => 0x7d891592e3cb0ebc => 353
	i64 9131857290992441898, ; 434: Xamarin.AndroidX.Camera.Core => 0x7ebadfd2d12a5a2a => 249
	i64 9138683372487561558, ; 435: System.Security.Cryptography.Csp => 0x7ed3201bc3e3d156 => 122
	i64 9171069561746690957, ; 436: it\StreamJsonRpc.resources => 0x7f462f2d0e4f138d => 439
	i64 9312692141327339315, ; 437: Xamarin.AndroidX.ViewPager2 => 0x813d54296a634f33 => 308
	i64 9324707631942237306, ; 438: Xamarin.AndroidX.AppCompat => 0x8168042fd44a7c7a => 243
	i64 9360039450522429843, ; 439: fr/Microsoft.ServiceHub.Framework.resources.dll => 0x81e58a49e4082993 => 359
	i64 9468215723722196442, ; 440: System.Xml.XPath.XDocument.dll => 0x8365dc09353ac5da => 160
	i64 9554839972845591462, ; 441: System.ServiceModel.Web => 0x84999c54e32a1ba6 => 132
	i64 9575902398040817096, ; 442: Xamarin.Google.Crypto.Tink.Android.dll => 0x84e4707ee708bdc8 => 314
	i64 9584643793929893533, ; 443: System.IO.dll => 0x85037ebfbbd7f69d => 58
	i64 9630107140230300349, ; 444: de\Microsoft.VisualStudio.Validation.resources => 0x85a5036bea6142bd => 423
	i64 9659729154652888475, ; 445: System.Text.RegularExpressions => 0x860e407c9991dd9b => 139
	i64 9662334977499516867, ; 446: System.Numerics.dll => 0x8617827802b0cfc3 => 84
	i64 9667360217193089419, ; 447: System.Diagnostics.StackTrace => 0x86295ce5cd89898b => 31
	i64 9678050649315576968, ; 448: Xamarin.AndroidX.CoordinatorLayout.dll => 0x864f57c9feb18c88 => 260
	i64 9679978064620295023, ; 449: it/StreamJsonRpc.resources.dll => 0x865630c35744136f => 439
	i64 9702891218465930390, ; 450: System.Collections.NonGeneric.dll => 0x86a79827b2eb3c96 => 11
	i64 9776375771063220978, ; 451: Nerdbank.Streams => 0x87aca9f760f57af2 => 213
	i64 9780093022148426479, ; 452: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x87b9dec9576efaef => 310
	i64 9808709177481450983, ; 453: Mono.Android.dll => 0x881f890734e555e7 => 172
	i64 9823691751235222432, ; 454: zh-Hant\Microsoft.ServiceHub.Framework.resources => 0x8854c3997f4f7ba0 => 368
	i64 9825649861376906464, ; 455: Xamarin.AndroidX.Concurrent.Futures => 0x885bb87d8abc94e0 => 257
	i64 9827080095733250167, ; 456: zh-Hant/Microsoft.ServiceHub.Framework.resources.dll => 0x8860cd47ed7df877 => 368
	i64 9834056768316610435, ; 457: System.Transactions.dll => 0x8879968718899783 => 151
	i64 9836529246295212050, ; 458: System.Reflection.Metadata => 0x88825f3bbc2ac012 => 95
	i64 9844193899530095610, ; 459: ja\Microsoft.VisualStudio.Validation.resources => 0x889d9a31e18f0bfa => 427
	i64 9907349773706910547, ; 460: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x897dfa20b758db53 => 270
	i64 9933555792566666578, ; 461: System.Linq.Queryable.dll => 0x89db145cf475c552 => 61
	i64 9956195530459977388, ; 462: Microsoft.Maui => 0x8a2b8315b36616ac => 198
	i64 9959489431142554298, ; 463: System.CodeDom => 0x8a3736deb7825aba => 217
	i64 9963664739326433656, ; 464: zh-Hans\Microsoft.ServiceHub.Framework.resources => 0x8a460c4a68a31978 => 367
	i64 9974604633896246661, ; 465: System.Xml.Serialization.dll => 0x8a6cea111a59dd85 => 158
	i64 9976963888860496507, ; 466: Microsoft.VisualStudio.Telemetry.dll => 0x8a754bcbf46e027b => 207
	i64 9991543690424095600, ; 467: es/Microsoft.Maui.Controls.resources.dll => 0x8aa9180c89861370 => 328
	i64 10017511394021241210, ; 468: Microsoft.Extensions.Logging.Debug => 0x8b055989ae10717a => 191
	i64 10038780035334861115, ; 469: System.Net.Http.dll => 0x8b50e941206af13b => 65
	i64 10051358222726253779, ; 470: System.Private.Xml => 0x8b7d990c97ccccd3 => 89
	i64 10078727084704864206, ; 471: System.Net.WebSockets.Client => 0x8bded4e257f117ce => 80
	i64 10089571585547156312, ; 472: System.IO.FileSystem.AccessControl => 0x8c055be67469bb58 => 48
	i64 10092835686693276772, ; 473: Microsoft.Maui.Controls => 0x8c10f49539bd0c64 => 196
	i64 10105485790837105934, ; 474: System.Threading.Tasks.Parallel => 0x8c3de5c91d9a650e => 144
	i64 10143853363526200146, ; 475: da\Microsoft.Maui.Controls.resources => 0x8cc634e3c2a16b52 => 325
	i64 10151361150077311800, ; 476: Microsoft.VisualStudio.Utilities.Internal.dll => 0x8ce0e12e890f1b38 => 210
	i64 10205853378024263619, ; 477: Microsoft.Extensions.Configuration.Binder => 0x8da279930adb4fc3 => 183
	i64 10224491876966872357, ; 478: ko/Microsoft.VisualStudio.Composition.resources.dll => 0x8de4b130bf7a3525 => 388
	i64 10229024438826829339, ; 479: Xamarin.AndroidX.CustomView => 0x8df4cb880b10061b => 264
	i64 10236703004850800690, ; 480: System.Net.ServicePoint.dll => 0x8e101325834e4832 => 75
	i64 10245369515835430794, ; 481: System.Reflection.Emit.Lightweight => 0x8e2edd4ad7fc978a => 92
	i64 10321854143672141184, ; 482: Xamarin.Jetbrains.Annotations.dll => 0x8f3e97a7f8f8c580 => 317
	i64 10360651442923773544, ; 483: System.Text.Encoding => 0x8fc86d98211c1e68 => 136
	i64 10364469296367737616, ; 484: System.Reflection.Emit.ILGeneration.dll => 0x8fd5fde967711b10 => 91
	i64 10376576884623852283, ; 485: Xamarin.AndroidX.Tracing.Tracing => 0x900101b2f888c2fb => 302
	i64 10406448008575299332, ; 486: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x906b2153fcb3af04 => 321
	i64 10407550536276267806, ; 487: zh-Hant/Microsoft.VisualStudio.Composition.resources.dll => 0x906f0c1238ac531e => 394
	i64 10430153318873392755, ; 488: Xamarin.AndroidX.Core => 0x90bf592ea44f6673 => 261
	i64 10447615475279048428, ; 489: de\Microsoft.VisualStudio.Utilities.resources => 0x90fd62ebc1e1aeec => 409
	i64 10448061532586656644, ; 490: System.Composition.Convention.dll => 0x90fef89b91404384 => 220
	i64 10505221885553605938, ; 491: ja/Microsoft.VisualStudio.Composition.resources.dll => 0x91ca0ba55714fd32 => 387
	i64 10506226065143327199, ; 492: ca\Microsoft.Maui.Controls.resources => 0x91cd9cf11ed169df => 323
	i64 10546663366131771576, ; 493: System.Runtime.Serialization.Json.dll => 0x925d4673efe8e8b8 => 113
	i64 10566960649245365243, ; 494: System.Globalization.dll => 0x92a562b96dcd13fb => 43
	i64 10595762989148858956, ; 495: System.Xml.XPath.XDocument => 0x930bb64cc472ea4c => 160
	i64 10670374202010151210, ; 496: Microsoft.Win32.Primitives.dll => 0x9414c8cd7b4ea92a => 5
	i64 10714184849103829812, ; 497: System.Runtime.Extensions.dll => 0x94b06e5aa4b4bb34 => 104
	i64 10785150219063592792, ; 498: System.Net.Primitives => 0x95ac8cfb68830758 => 71
	i64 10809043855025277762, ; 499: Microsoft.Extensions.Options.ConfigurationExtensions => 0x9601701e0c668b42 => 193
	i64 10822644899632537592, ; 500: System.Linq.Queryable => 0x9631c23204ca5ff8 => 61
	i64 10830817578243619689, ; 501: System.Formats.Tar => 0x964ecb340a447b69 => 40
	i64 10835442135346987677, ; 502: pl/Microsoft.VisualStudio.Utilities.resources.dll => 0x965f3936a5a8429d => 416
	i64 10843446450579966237, ; 503: Microsoft.VisualStudio.Composition => 0x967ba91883ca091d => 204
	i64 10847732767863316357, ; 504: Xamarin.AndroidX.Arch.Core.Common => 0x968ae37a86db9f85 => 245
	i64 10880838204485145808, ; 505: CommunityToolkit.Maui.dll => 0x970080b2a4d614d0 => 174
	i64 10899834349646441345, ; 506: System.Web => 0x9743fd975946eb81 => 154
	i64 10943875058216066601, ; 507: System.IO.UnmanagedMemoryStream.dll => 0x97e07461df39de29 => 57
	i64 10964653383833615866, ; 508: System.Diagnostics.Tracing => 0x982a4628ccaffdfa => 35
	i64 10991732737396536896, ; 509: zh-Hant/Microsoft.VisualStudio.Utilities.resources.dll => 0x988a7ab01e8d7640 => 421
	i64 11002576679268595294, ; 510: Microsoft.Extensions.Logging.Abstractions => 0x98b1013215cd365e => 190
	i64 11009005086950030778, ; 511: Microsoft.Maui.dll => 0x98c7d7cc621ffdba => 198
	i64 11019817191295005410, ; 512: Xamarin.AndroidX.Annotation.Jvm.dll => 0x98ee415998e1b2e2 => 242
	i64 11023048688141570732, ; 513: System.Core => 0x98f9bc61168392ac => 22
	i64 11037814507248023548, ; 514: System.Xml => 0x992e31d0412bf7fc => 164
	i64 11047101296015504039, ; 515: Microsoft.Win32.SystemEvents => 0x994f301942c2f2a7 => 212
	i64 11071824625609515081, ; 516: Xamarin.Google.ErrorProne.Annotations => 0x99a705d600e0a049 => 315
	i64 11078422477095414952, ; 517: MessagePack.Annotations => 0x99be768c02f9aca8 => 180
	i64 11079569176516935993, ; 518: ko\Microsoft.ServiceHub.Resources => 0x99c28976c6a8c139 => 375
	i64 11103970607964515343, ; 519: hu\Microsoft.Maui.Controls.resources => 0x9a193a6fc41a6c0f => 334
	i64 11128108736298314439, ; 520: ja\Microsoft.ServiceHub.Resources => 0x9a6efbefb8fb6ac7 => 374
	i64 11129765940661776577, ; 521: es\Microsoft.VisualStudio.Composition.resources => 0x9a74df27ae62f0c1 => 384
	i64 11136029745144976707, ; 522: Jsr305Binding.dll => 0x9a8b200d4f8cd543 => 313
	i64 11138965398324199724, ; 523: ru/Microsoft.VisualStudio.Composition.resources.dll => 0x9a958e03547b412c => 391
	i64 11162124722117608902, ; 524: Xamarin.AndroidX.ViewPager => 0x9ae7d54b986d05c6 => 307
	i64 11188319605227840848, ; 525: System.Threading.Overlapped => 0x9b44e5671724e550 => 141
	i64 11220793807500858938, ; 526: ja\Microsoft.Maui.Controls.resources => 0x9bb8448481fdd63a => 337
	i64 11226290749488709958, ; 527: Microsoft.Extensions.Options.dll => 0x9bcbcbf50c874146 => 192
	i64 11235648312900863002, ; 528: System.Reflection.DispatchProxy.dll => 0x9bed0a9c8fac441a => 90
	i64 11299661109949763898, ; 529: Xamarin.AndroidX.Collection.Jvm => 0x9cd075e94cda113a => 255
	i64 11329751333533450475, ; 530: System.Threading.Timer.dll => 0x9d3b5ccf6cc500eb => 148
	i64 11340910727871153756, ; 531: Xamarin.AndroidX.CursorAdapter => 0x9d630238642d465c => 263
	i64 11341245327015630248, ; 532: System.Configuration.ConfigurationManager.dll => 0x9d643289535355a8 => 224
	i64 11347436699239206956, ; 533: System.Xml.XmlSerializer.dll => 0x9d7a318e8162502c => 163
	i64 11385656703440359675, ; 534: zh-Hant\Microsoft.VisualStudio.Validation.resources => 0x9e01fa72af1b90fb => 434
	i64 11392833485892708388, ; 535: Xamarin.AndroidX.Print.dll => 0x9e1b79b18fcf6824 => 292
	i64 11426995335776520728, ; 536: it/Microsoft.ServiceHub.Resources.dll => 0x9e94d7b70f014a18 => 373
	i64 11432086877643258359, ; 537: cs\Microsoft.ServiceHub.Resources => 0x9ea6ee720754b9f7 => 369
	i64 11432101114902388181, ; 538: System.AppContext => 0x9ea6fb64e61a9dd5 => 7
	i64 11446671985764974897, ; 539: Mono.Android.Export => 0x9edabf8623efc131 => 170
	i64 11448276831755070604, ; 540: System.Diagnostics.TextWriterTraceListener => 0x9ee0731f77186c8c => 32
	i64 11465237565819292444, ; 541: ru/Microsoft.VisualStudio.Threading.resources.dll => 0x9f1cb4d2181f8b1c => 404
	i64 11482691884102096350, ; 542: fr/Microsoft.VisualStudio.Threading.resources.dll => 0x9f5ab76e412a95de => 398
	i64 11485890710487134646, ; 543: System.Runtime.InteropServices => 0x9f6614bf0f8b71b6 => 108
	i64 11508496261504176197, ; 544: Xamarin.AndroidX.Fragment.Ktx.dll => 0x9fb664600dde1045 => 273
	i64 11513602507638267977, ; 545: System.IO.Pipelines.dll => 0x9fc8887aa0d36049 => 228
	i64 11518296021396496455, ; 546: id\Microsoft.Maui.Controls.resources => 0x9fd9353475222047 => 335
	i64 11529969570048099689, ; 547: Xamarin.AndroidX.ViewPager.dll => 0xa002ae3c4dc7c569 => 307
	i64 11530571088791430846, ; 548: Microsoft.Extensions.Logging => 0xa004d1504ccd66be => 189
	i64 11580057168383206117, ; 549: Xamarin.AndroidX.Annotation => 0xa0b4a0a4103262e5 => 240
	i64 11591352189662810718, ; 550: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0xa0dcc167234c525e => 300
	i64 11597174155934063391, ; 551: fr\Microsoft.VisualStudio.Utilities.resources => 0xa0f170734aca5b1f => 412
	i64 11597940890313164233, ; 552: netstandard => 0xa0f429ca8d1805c9 => 168
	i64 11633085459938445148, ; 553: de/Microsoft.VisualStudio.Composition.resources.dll => 0xa17105975164875c => 383
	i64 11672361001936329215, ; 554: Xamarin.AndroidX.Interpolator => 0xa1fc8e7d0a8999ff => 274
	i64 11692977985522001935, ; 555: System.Threading.Overlapped.dll => 0xa245cd869980680f => 141
	i64 11705530742807338875, ; 556: he/Microsoft.Maui.Controls.resources.dll => 0xa272663128721f7b => 331
	i64 11707554492040141440, ; 557: System.Linq.Parallel.dll => 0xa27996c7fe94da80 => 60
	i64 11743665907891708234, ; 558: System.Threading.Tasks => 0xa2f9e1ec30c0214a => 145
	i64 11766809059788673038, ; 559: zh-Hans/Microsoft.VisualStudio.Threading.resources.dll => 0xa34c1a7f248d500e => 406
	i64 11868448598187452935, ; 560: CommunityToolkit.Maui.Camera.dll => 0xa4b5331e49f22a07 => 175
	i64 11890655789319506262, ; 561: ko/StreamJsonRpc.resources.dll => 0xa5041870e4d4c956 => 441
	i64 11938677804301963148, ; 562: tr/Microsoft.ServiceHub.Framework.resources.dll => 0xa5aeb4358d424f8c => 366
	i64 11991047634523762324, ; 563: System.Net => 0xa668c24ad493ae94 => 82
	i64 12008693925152623904, ; 564: pl\Microsoft.VisualStudio.Utilities.resources => 0xa6a77380091d0d20 => 416
	i64 12011556116648931059, ; 565: System.Security.Cryptography.ProtectedData => 0xa6b19ea5ec87aef3 => 230
	i64 12040886584167504988, ; 566: System.Net.ServicePoint => 0xa719d28d8e121c5c => 75
	i64 12063623837170009990, ; 567: System.Security => 0xa76a99f6ce740786 => 131
	i64 12096697103934194533, ; 568: System.Diagnostics.Contracts => 0xa7e019eccb7e8365 => 26
	i64 12102847907131387746, ; 569: System.Buffers => 0xa7f5f40c43256f62 => 8
	i64 12123043025855404482, ; 570: System.Reflection.Extensions.dll => 0xa83db366c0e359c2 => 94
	i64 12124151557889122664, ; 571: pt-BR\Microsoft.VisualStudio.Utilities.resources => 0xa841a39afafae168 => 417
	i64 12137774235383566651, ; 572: Xamarin.AndroidX.VectorDrawable => 0xa872095bbfed113b => 304
	i64 12145679461940342714, ; 573: System.Text.Json => 0xa88e1f1ebcb62fba => 138
	i64 12191646537372739477, ; 574: Xamarin.Android.Glide.dll => 0xa9316dee7f392795 => 234
	i64 12201331334810686224, ; 575: System.Runtime.Serialization.Primitives.dll => 0xa953d6341e3bd310 => 114
	i64 12269460666702402136, ; 576: System.Collections.Immutable.dll => 0xaa45e178506c9258 => 10
	i64 12313367145828839434, ; 577: System.IO.Pipelines => 0xaae1de2e1c17f00a => 228
	i64 12332222936682028543, ; 578: System.Runtime.Handles => 0xab24db6c07db5dff => 105
	i64 12341818387765915815, ; 579: CommunityToolkit.Maui.Core.dll => 0xab46f26f152bf0a7 => 176
	i64 12374012165002318533, ; 580: cs\Microsoft.VisualStudio.Composition.resources => 0xabb95280f55352c5 => 382
	i64 12375446203996702057, ; 581: System.Configuration.dll => 0xabbe6ac12e2e0569 => 20
	i64 12451044538927396471, ; 582: Xamarin.AndroidX.Fragment.dll => 0xaccaff0a2955b677 => 272
	i64 12466513435562512481, ; 583: Xamarin.AndroidX.Loader.dll => 0xad01f3eb52569061 => 286
	i64 12471568088487385307, ; 584: fr\Microsoft.ServiceHub.Resources => 0xad13e9196a6358db => 372
	i64 12475113361194491050, ; 585: _Microsoft.Android.Resource.Designer.dll => 0xad2081818aba1caa => 448
	i64 12481884125606176949, ; 586: zh-Hant/StreamJsonRpc.resources.dll => 0xad388f7afa57e8b5 => 447
	i64 12487638416075308985, ; 587: Xamarin.AndroidX.DocumentFile.dll => 0xad4d00fa21b0bfb9 => 266
	i64 12517810545449516888, ; 588: System.Diagnostics.TraceSource.dll => 0xadb8325e6f283f58 => 34
	i64 12538491095302438457, ; 589: Xamarin.AndroidX.CardView.dll => 0xae01ab382ae67e39 => 253
	i64 12550732019250633519, ; 590: System.IO.Compression => 0xae2d28465e8e1b2f => 47
	i64 12573751959656727467, ; 591: ru/Microsoft.VisualStudio.Validation.resources.dll => 0xae7ef0c9e93603ab => 431
	i64 12603216663576844694, ; 592: de/Microsoft.VisualStudio.Threading.resources.dll => 0xaee79ec7e3c81996 => 396
	i64 12681088699309157496, ; 593: it/Microsoft.Maui.Controls.resources.dll => 0xaffc46fc178aec78 => 336
	i64 12699999919562409296, ; 594: System.Diagnostics.StackTrace.dll => 0xb03f76a3ad01c550 => 31
	i64 12700543734426720211, ; 595: Xamarin.AndroidX.Collection => 0xb041653c70d157d3 => 254
	i64 12708238894395270091, ; 596: System.IO => 0xb05cbbf17d3ba3cb => 58
	i64 12708922737231849740, ; 597: System.Text.Encoding.Extensions => 0xb05f29e50e96e90c => 135
	i64 12717050818822477433, ; 598: System.Runtime.Serialization.Xml.dll => 0xb07c0a5786811679 => 115
	i64 12726057104018228727, ; 599: Microsoft.NET.StringTools => 0xb09c0982b457c5f7 => 201
	i64 12753841065332862057, ; 600: Xamarin.AndroidX.Window => 0xb0febee04cf46c69 => 309
	i64 12760970142902902754, ; 601: Xamarin.AndroidX.Camera.Lifecycle.dll => 0xb11812bc0517dfe2 => 250
	i64 12808034783427285688, ; 602: cs\StreamJsonRpc.resources => 0xb1bf47c69107d2b8 => 435
	i64 12823819093633476069, ; 603: th/Microsoft.Maui.Controls.resources.dll => 0xb1f75b85abe525e5 => 349
	i64 12835242264250840079, ; 604: System.IO.Pipes => 0xb21ff0d5d6c0740f => 56
	i64 12843321153144804894, ; 605: Microsoft.Extensions.Primitives => 0xb23ca48abd74d61e => 194
	i64 12843770487262409629, ; 606: System.AppContext.dll => 0xb23e3d357debf39d => 7
	i64 12848781286208560954, ; 607: Microsoft.VisualStudio.Telemetry => 0xb2500a810925df3a => 207
	i64 12859557719246324186, ; 608: System.Net.WebHeaderCollection.dll => 0xb276539ce04f41da => 78
	i64 12928116100519456705, ; 609: es\Microsoft.VisualStudio.Utilities.resources => 0xb369e518ea786fc1 => 411
	i64 12963446364377008305, ; 610: System.Drawing.Common.dll => 0xb3e769c8fd8548b1 => 227
	i64 12982280885948128408, ; 611: Xamarin.AndroidX.CustomView.PoolingContainer => 0xb42a53aec5481c98 => 265
	i64 13000926602004112928, ; 612: zh-Hans\Microsoft.VisualStudio.Composition.resources => 0xb46c91dcd761a620 => 393
	i64 13019838822340531700, ; 613: cs/Microsoft.VisualStudio.Composition.resources.dll => 0xb4afc26d467295f4 => 382
	i64 13068258254871114833, ; 614: System.Runtime.Serialization.Formatters.dll => 0xb55bc7a4eaa8b451 => 112
	i64 13072259076476491058, ; 615: tr\StreamJsonRpc.resources => 0xb569fe5ec941d532 => 445
	i64 13126023683090012938, ; 616: System.Composition.TypedParts.dll => 0xb62900febff1db0a => 223
	i64 13129914918964716986, ; 617: Xamarin.AndroidX.Emoji2.dll => 0xb636d40db3fe65ba => 269
	i64 13130412034557475351, ; 618: fr\Microsoft.VisualStudio.Threading.resources => 0xb638982d724ffa17 => 398
	i64 13162471042547327635, ; 619: System.Security.Permissions => 0xb6aa7dace9662293 => 231
	i64 13173818576982874404, ; 620: System.Runtime.CompilerServices.VisualC.dll => 0xb6d2ce32a8819924 => 103
	i64 13221551921002590604, ; 621: ca/Microsoft.Maui.Controls.resources.dll => 0xb77c636bdebe318c => 323
	i64 13222659110913276082, ; 622: ja/Microsoft.Maui.Controls.resources.dll => 0xb78052679c1178b2 => 337
	i64 13244563641972016158, ; 623: pt-BR/Microsoft.ServiceHub.Framework.resources.dll => 0xb7ce2475accbc41e => 364
	i64 13343850469010654401, ; 624: Mono.Android.Runtime.dll => 0xb92ee14d854f44c1 => 171
	i64 13370592475155966277, ; 625: System.Runtime.Serialization => 0xb98de304062ea945 => 116
	i64 13381594904270902445, ; 626: he\Microsoft.Maui.Controls.resources => 0xb9b4f9aaad3e94ad => 331
	i64 13401370062847626945, ; 627: Xamarin.AndroidX.VectorDrawable.dll => 0xb9fb3b1193964ec1 => 304
	i64 13404347523447273790, ; 628: Xamarin.AndroidX.ConstraintLayout.Core => 0xba05cf0da4f6393e => 259
	i64 13431476299110033919, ; 629: System.Net.WebClient => 0xba663087f18829ff => 77
	i64 13454009404024712428, ; 630: Xamarin.Google.Guava.ListenableFuture => 0xbab63e4543a86cec => 316
	i64 13463706743370286408, ; 631: System.Private.DataContractSerialization.dll => 0xbad8b1f3069e0548 => 86
	i64 13465488254036897740, ; 632: Xamarin.Kotlin.StdLib => 0xbadf06394d106fcc => 318
	i64 13467053111158216594, ; 633: uk/Microsoft.Maui.Controls.resources.dll => 0xbae49573fde79792 => 351
	i64 13491513212026656886, ; 634: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0xbb3b7bc905569876 => 246
	i64 13502641473732064860, ; 635: System.Composition.AttributedModel => 0xbb6304e15b41b65c => 219
	i64 13540124433173649601, ; 636: vi\Microsoft.Maui.Controls.resources => 0xbbe82f6eede718c1 => 352
	i64 13545416393490209236, ; 637: id/Microsoft.Maui.Controls.resources.dll => 0xbbfafc7174bc99d4 => 335
	i64 13572454107664307259, ; 638: Xamarin.AndroidX.RecyclerView.dll => 0xbc5b0b19d99f543b => 294
	i64 13578472628727169633, ; 639: System.Xml.XPath => 0xbc706ce9fba5c261 => 161
	i64 13580399111273692417, ; 640: Microsoft.VisualBasic.Core.dll => 0xbc77450a277fbd01 => 3
	i64 13621154251410165619, ; 641: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0xbd080f9faa1acf73 => 265
	i64 13647894001087880694, ; 642: System.Data.dll => 0xbd670f48cb071df6 => 25
	i64 13675589307506966157, ; 643: Xamarin.AndroidX.Activity.Ktx => 0xbdc97404d0153e8d => 239
	i64 13702626353344114072, ; 644: System.Diagnostics.Tools.dll => 0xbe29821198fb6d98 => 33
	i64 13703125838636057883, ; 645: Microsoft.VisualStudio.Validation.dll => 0xbe2b48591461051b => 211
	i64 13710614125866346983, ; 646: System.Security.AccessControl.dll => 0xbe45e2e7d0b769e7 => 118
	i64 13713329104121190199, ; 647: System.Dynamic.Runtime => 0xbe4f8829f32b5737 => 38
	i64 13717397318615465333, ; 648: System.ComponentModel.Primitives.dll => 0xbe5dfc2ef2f87d75 => 17
	i64 13755568601956062840, ; 649: fr/Microsoft.Maui.Controls.resources.dll => 0xbee598c36b1b9678 => 330
	i64 13768883594457632599, ; 650: System.IO.IsolatedStorage => 0xbf14e6adb159cf57 => 53
	i64 13814445057219246765, ; 651: hr/Microsoft.Maui.Controls.resources.dll => 0xbfb6c49664b43aad => 333
	i64 13819460841644211703, ; 652: es\Microsoft.ServiceHub.Resources => 0xbfc8966ab59fbdf7 => 371
	i64 13859681129916375842, ; 653: tr\Microsoft.VisualStudio.Validation.resources => 0xc0577a8e50d9a322 => 432
	i64 13881769479078963060, ; 654: System.Console.dll => 0xc0a5f3cade5c6774 => 21
	i64 13882652712560114096, ; 655: System.Windows.Extensions.dll => 0xc0a91716b04239b0 => 233
	i64 13911222732217019342, ; 656: System.Security.Cryptography.OpenSsl.dll => 0xc10e975ec1226bce => 124
	i64 13928444506500929300, ; 657: System.Windows.dll => 0xc14bc67b8bba9714 => 155
	i64 13959074834287824816, ; 658: Xamarin.AndroidX.Fragment => 0xc1b8989a7ad20fb0 => 272
	i64 13988492781996941750, ; 659: ja\Microsoft.ServiceHub.Framework.resources => 0xc2211c122e0b21b6 => 361
	i64 14043772898905986081, ; 660: tr/Microsoft.VisualStudio.Utilities.resources.dll => 0xc2e5810b618c9421 => 419
	i64 14075334701871371868, ; 661: System.ServiceModel.Web.dll => 0xc355a25647c5965c => 132
	i64 14100563506285742564, ; 662: da/Microsoft.Maui.Controls.resources.dll => 0xc3af43cd0cff89e4 => 325
	i64 14124974489674258913, ; 663: Xamarin.AndroidX.CardView => 0xc405fd76067d19e1 => 253
	i64 14125464355221830302, ; 664: System.Threading.dll => 0xc407bafdbc707a9e => 149
	i64 14175074915974767561, ; 665: zh-Hans\Microsoft.ServiceHub.Resources => 0xc4b7fb888739e3c9 => 380
	i64 14178052285788134900, ; 666: Xamarin.Android.Glide.Annotations.dll => 0xc4c28f6f75511df4 => 235
	i64 14212104595480609394, ; 667: System.Security.Cryptography.Cng.dll => 0xc53b89d4a4518272 => 121
	i64 14220608275227875801, ; 668: System.Diagnostics.FileVersionInfo.dll => 0xc559bfe1def019d9 => 29
	i64 14226382999226559092, ; 669: System.ServiceProcess => 0xc56e43f6938e2a74 => 133
	i64 14232023429000439693, ; 670: System.Resources.Writer.dll => 0xc5824de7789ba78d => 101
	i64 14254574811015963973, ; 671: System.Text.Encoding.Extensions.dll => 0xc5d26c4442d66545 => 135
	i64 14261073672896646636, ; 672: Xamarin.AndroidX.Print => 0xc5e982f274ae0dec => 292
	i64 14298246716367104064, ; 673: System.Web.dll => 0xc66d93a217f4e840 => 154
	i64 14316535876961222820, ; 674: Xamarin.AndroidX.Camera.Camera2 => 0xc6ae8d872068c0a4 => 248
	i64 14327695147300244862, ; 675: System.Reflection.dll => 0xc6d632d338eb4d7e => 98
	i64 14327709162229390963, ; 676: System.Security.Cryptography.X509Certificates => 0xc6d63f9253cade73 => 126
	i64 14330684041385202088, ; 677: pt-BR/Microsoft.VisualStudio.Threading.resources.dll => 0xc6e0d1355b1fd1a8 => 403
	i64 14331727281556788554, ; 678: Xamarin.Android.Glide.DiskLruCache.dll => 0xc6e48607a2f7954a => 236
	i64 14346402571976470310, ; 679: System.Net.Ping.dll => 0xc718a920f3686f26 => 70
	i64 14370252628338276234, ; 680: tr/Microsoft.VisualStudio.Validation.resources.dll => 0xc76d64a0ecb9f78a => 432
	i64 14391267691003248864, ; 681: Nerdbank.Streams.dll => 0xc7b80db77dce00e0 => 213
	i64 14415297290983905433, ; 682: pt-BR\StreamJsonRpc.resources => 0xc80d6c82b8bf1899 => 443
	i64 14452298576064698508, ; 683: es\Microsoft.VisualStudio.Validation.resources => 0xc890e0fbbe38588c => 424
	i64 14461014870687870182, ; 684: System.Net.Requests.dll => 0xc8afd8683afdece6 => 73
	i64 14464374589798375073, ; 685: ru\Microsoft.Maui.Controls.resources => 0xc8bbc80dcb1e5ea1 => 346
	i64 14486659737292545672, ; 686: Xamarin.AndroidX.Lifecycle.LiveData => 0xc90af44707469e88 => 277
	i64 14495724990987328804, ; 687: Xamarin.AndroidX.ResourceInspection.Annotation => 0xc92b2913e18d5d24 => 295
	i64 14522721392235705434, ; 688: el/Microsoft.Maui.Controls.resources.dll => 0xc98b12295c2cf45a => 327
	i64 14550104935177758553, ; 689: Xamarin.AndroidX.Camera.Video.dll => 0xc9ec5b5949eda759 => 251
	i64 14551742072151931844, ; 690: System.Text.Encodings.Web.dll => 0xc9f22c50f1b8fbc4 => 137
	i64 14556034074661724008, ; 691: CommunityToolkit.Maui.Core => 0xca016bdea6b19f68 => 176
	i64 14561513370130550166, ; 692: System.Security.Cryptography.Primitives.dll => 0xca14e3428abb8d96 => 125
	i64 14574160591280636898, ; 693: System.Net.Quic => 0xca41d1d72ec783e2 => 72
	i64 14614774114972620539, ; 694: zh-Hans/Microsoft.VisualStudio.Composition.resources.dll => 0xcad21ba00b8456fb => 393
	i64 14622043554576106986, ; 695: System.Runtime.Serialization.Formatters => 0xcaebef2458cc85ea => 112
	i64 14636892923918804708, ; 696: ja/StreamJsonRpc.resources.dll => 0xcb20b090919be6e4 => 440
	i64 14644440854989303794, ; 697: Xamarin.AndroidX.LocalBroadcastManager.dll => 0xcb3b815e37daeff2 => 287
	i64 14656309759869513278, ; 698: Microsoft.VisualStudio.RemoteControl.dll => 0xcb65ac12fb1cee3e => 205
	i64 14669215534098758659, ; 699: Microsoft.Extensions.DependencyInjection.dll => 0xcb9385ceb3993c03 => 184
	i64 14690985099581930927, ; 700: System.Web.HttpUtility => 0xcbe0dd1ca5233daf => 153
	i64 14705122255218365489, ; 701: ko\Microsoft.Maui.Controls.resources => 0xcc1316c7b0fb5431 => 338
	i64 14744092281598614090, ; 702: zh-Hans\Microsoft.Maui.Controls.resources => 0xcc9d89d004439a4a => 354
	i64 14753596820584604397, ; 703: zh-Hans/StreamJsonRpc.resources.dll => 0xccbf4e23f24722ed => 446
	i64 14771404191615744121, ; 704: pl\Microsoft.VisualStudio.Composition.resources => 0xccfe91d99af8f879 => 389
	i64 14792063746108907174, ; 705: Xamarin.Google.Guava.ListenableFuture.dll => 0xcd47f79af9c15ea6 => 316
	i64 14819209644871321045, ; 706: zh-Hans/Microsoft.VisualStudio.Utilities.resources.dll => 0xcda868a80e9905d5 => 420
	i64 14820817770416679739, ; 707: tr\Microsoft.VisualStudio.Threading.resources => 0xcdae1f3cf67bab3b => 405
	i64 14832630590065248058, ; 708: System.Security.Claims => 0xcdd816ef5d6e873a => 119
	i64 14847150824983734404, ; 709: Microsoft.VisualStudio.RpcContracts => 0xce0bad0301cda884 => 206
	i64 14852515768018889994, ; 710: Xamarin.AndroidX.CursorAdapter.dll => 0xce1ebc6625a76d0a => 263
	i64 14889905118082851278, ; 711: GoogleGson.dll => 0xcea391d0969961ce => 178
	i64 14892012299694389861, ; 712: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xceab0e490a083a65 => 355
	i64 14904040806490515477, ; 713: ar\Microsoft.Maui.Controls.resources => 0xced5ca2604cb2815 => 322
	i64 14912225920358050525, ; 714: System.Security.Principal.Windows => 0xcef2de7759506add => 128
	i64 14935719434541007538, ; 715: System.Text.Encoding.CodePages.dll => 0xcf4655b160b702b2 => 134
	i64 14954917835170835695, ; 716: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xcf8a8a895a82ecef => 185
	i64 14984936317414011727, ; 717: System.Net.WebHeaderCollection => 0xcff5302fe54ff34f => 78
	i64 14987728460634540364, ; 718: System.IO.Compression.dll => 0xcfff1ba06622494c => 47
	i64 14988210264188246988, ; 719: Xamarin.AndroidX.DocumentFile => 0xd000d1d307cddbcc => 266
	i64 15015154896917945444, ; 720: System.Net.Security.dll => 0xd0608bd33642dc64 => 74
	i64 15023260263817286165, ; 721: es/StreamJsonRpc.resources.dll => 0xd07d579d01121215 => 437
	i64 15023528575408771796, ; 722: ko\StreamJsonRpc.resources => 0xd07e4ba42a2b92d4 => 441
	i64 15024878362326791334, ; 723: System.Net.Http.Json => 0xd0831743ebf0f4a6 => 64
	i64 15044901218619597537, ; 724: en\Microsoft.VisualStudio.Utilities.resources => 0xd0ca39f270a9a2e1 => 410
	i64 15051741671811457419, ; 725: Microsoft.Extensions.Diagnostics.Abstractions.dll => 0xd0e2874d8f44218b => 187
	i64 15071021337266399595, ; 726: System.Resources.Reader.dll => 0xd127060e7a18a96b => 99
	i64 15076659072870671916, ; 727: System.ObjectModel.dll => 0xd13b0d8c1620662c => 85
	i64 15078030747917693565, ; 728: pl/Microsoft.VisualStudio.Threading.resources.dll => 0xd13fed14120d327d => 402
	i64 15111608613780139878, ; 729: ms\Microsoft.Maui.Controls.resources => 0xd1b737f831192f66 => 339
	i64 15115185479366240210, ; 730: System.IO.Compression.Brotli.dll => 0xd1c3ed1c1bc467d2 => 44
	i64 15133485256822086103, ; 731: System.Linq.dll => 0xd204f0a9127dd9d7 => 62
	i64 15150743910298169673, ; 732: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xd2424150783c3149 => 293
	i64 15201252454341482878, ; 733: ja\StreamJsonRpc.resources => 0xd2f5b2914a0f197e => 440
	i64 15227001540531775957, ; 734: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd3512d3999b8e9d5 => 182
	i64 15234786388537674379, ; 735: System.Dynamic.Runtime.dll => 0xd36cd580c5be8a8b => 38
	i64 15237913642941303365, ; 736: pl\Microsoft.ServiceHub.Framework.resources => 0xd377f1b96d984a45 => 363
	i64 15249474349747396247, ; 737: zh-Hans\StreamJsonRpc.resources => 0xd3a104203c1fde97 => 446
	i64 15250465174479574862, ; 738: System.Globalization.Calendars.dll => 0xd3a489469852174e => 41
	i64 15272359115529052076, ; 739: Xamarin.AndroidX.Collection.Ktx => 0xd3f251b2fb4edfac => 256
	i64 15279429628684179188, ; 740: Xamarin.KotlinX.Coroutines.Android.dll => 0xd40b704b1c4c96f4 => 319
	i64 15299439993936780255, ; 741: System.Xml.XPath.dll => 0xd452879d55019bdf => 161
	i64 15300862763834473199, ; 742: System.Composition.Hosting.dll => 0xd457959dc35afaef => 221
	i64 15338463749992804988, ; 743: System.Resources.Reader => 0xd4dd2b839286f27c => 99
	i64 15352427450275134006, ; 744: System.Composition.Runtime.dll => 0xd50ec76ce59afa36 => 222
	i64 15370334346939861994, ; 745: Xamarin.AndroidX.Core.dll => 0xd54e65a72c560bea => 261
	i64 15391712275433856905, ; 746: Microsoft.Extensions.DependencyInjection.Abstractions => 0xd59a58c406411f89 => 185
	i64 15455258289457179632, ; 747: pl/Microsoft.ServiceHub.Resources.dll => 0xd67c1b875f01abf0 => 376
	i64 15475196252089753159, ; 748: System.Diagnostics.EventLog => 0xd6c2f1000b441e47 => 225
	i64 15485751695148966188, ; 749: pt-BR/Microsoft.ServiceHub.Resources.dll => 0xd6e8711ea541e12c => 377
	i64 15526743539506359484, ; 750: System.Text.Encoding.dll => 0xd77a12fc26de2cbc => 136
	i64 15527772828719725935, ; 751: System.Console => 0xd77dbb1e38cd3d6f => 21
	i64 15530465045505749832, ; 752: System.Net.HttpListener.dll => 0xd7874bacc9fdb348 => 66
	i64 15536481058354060254, ; 753: de\Microsoft.Maui.Controls.resources => 0xd79cab34eec75bde => 326
	i64 15541854775306130054, ; 754: System.Security.Cryptography.X509Certificates.dll => 0xd7afc292e8d49286 => 126
	i64 15557562860424774966, ; 755: System.Net.Sockets => 0xd7e790fe7a6dc536 => 76
	i64 15558627161509849899, ; 756: ru\Microsoft.VisualStudio.Validation.resources => 0xd7eb58f86289eb2b => 431
	i64 15582737692548360875, ; 757: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xd841015ed86f6aab => 285
	i64 15609085926864131306, ; 758: System.dll => 0xd89e9cf3334914ea => 165
	i64 15661133872274321916, ; 759: System.Xml.ReaderWriter.dll => 0xd9578647d4bfb1fc => 157
	i64 15664356999916475676, ; 760: de/Microsoft.Maui.Controls.resources.dll => 0xd962f9b2b6ecd51c => 326
	i64 15686827046011320720, ; 761: it\Microsoft.VisualStudio.Utilities.resources => 0xd9b2ce16027eb990 => 413
	i64 15710114879900314733, ; 762: Microsoft.Win32.Registry => 0xda058a3f5d096c6d => 6
	i64 15743187114543869802, ; 763: hu/Microsoft.Maui.Controls.resources.dll => 0xda7b09450ae4ef6a => 334
	i64 15750144475371186037, ; 764: Xamarin.AndroidX.Camera.View.dll => 0xda93c0f3d794a775 => 252
	i64 15750798694401567573, ; 765: ru\Microsoft.VisualStudio.Threading.resources => 0xda9613f6147deb55 => 404
	i64 15755368083429170162, ; 766: System.IO.FileSystem.Primitives => 0xdaa64fcbde529bf2 => 50
	i64 15777549416145007739, ; 767: Xamarin.AndroidX.SlidingPaneLayout.dll => 0xdaf51d99d77eb47b => 299
	i64 15783653065526199428, ; 768: el\Microsoft.Maui.Controls.resources => 0xdb0accd674b1c484 => 327
	i64 15817206913877585035, ; 769: System.Threading.Tasks.dll => 0xdb8201e29086ac8b => 145
	i64 15847085070278954535, ; 770: System.Threading.Channels.dll => 0xdbec27e8f35f8e27 => 140
	i64 15878922146558839051, ; 771: CardGameCorner.dll => 0xdc5d438fe9b9090b => 1
	i64 15885744048853936810, ; 772: System.Resources.Writer => 0xdc75800bd0b6eaaa => 101
	i64 15903718333706527842, ; 773: zh-Hans/Microsoft.VisualStudio.Validation.resources.dll => 0xdcb55b902071e462 => 433
	i64 15928521404965645318, ; 774: Microsoft.Maui.Controls.Compatibility => 0xdd0d79d32c2eec06 => 195
	i64 15934062614519587357, ; 775: System.Security.Cryptography.OpenSsl => 0xdd2129868f45a21d => 124
	i64 15937190497610202713, ; 776: System.Security.Cryptography.Cng => 0xdd2c465197c97e59 => 121
	i64 15953812885175980916, ; 777: ko\Microsoft.VisualStudio.Validation.resources => 0xdd67544ac9f6d374 => 428
	i64 15963349826457351533, ; 778: System.Threading.Tasks.Extensions => 0xdd893616f748b56d => 143
	i64 15971679995444160383, ; 779: System.Formats.Tar.dll => 0xdda6ce5592a9677f => 40
	i64 16018552496348375205, ; 780: System.Net.NetworkInformation.dll => 0xde4d54a020caa8a5 => 69
	i64 16054465462676478687, ; 781: System.Globalization.Extensions => 0xdecceb47319bdadf => 42
	i64 16090829755721178160, ; 782: tr\Microsoft.ServiceHub.Framework.resources => 0xdf4e1c68f30bdc30 => 366
	i64 16091261637069827414, ; 783: ko/Microsoft.VisualStudio.Validation.resources.dll => 0xdf4fa534296aed56 => 428
	i64 16094225675758313977, ; 784: CardGameCorner => 0xdf5a2cfb328ba1f9 => 1
	i64 16131251062481977478, ; 785: pl\Microsoft.ServiceHub.Resources => 0xdfddb75fd1e18086 => 376
	i64 16154507427712707110, ; 786: System => 0xe03056ea4e39aa26 => 165
	i64 16177821557421402585, ; 787: StreamJsonRpc => 0xe0832afe21c269d9 => 216
	i64 16201041905702586336, ; 788: System.Diagnostics.PerformanceCounter => 0xe0d5a9c6c49ec7e0 => 226
	i64 16219561732052121626, ; 789: System.Net.Security => 0xe1177575db7c781a => 74
	i64 16236971995090710244, ; 790: pt-BR\Microsoft.VisualStudio.Composition.resources => 0xe15550009b58eee4 => 390
	i64 16288847719894691167, ; 791: nb\Microsoft.Maui.Controls.resources => 0xe20d9cb300c12d5f => 340
	i64 16315482530584035869, ; 792: WindowsBase.dll => 0xe26c3ceb1e8d821d => 166
	i64 16321164108206115771, ; 793: Microsoft.Extensions.Logging.Abstractions.dll => 0xe2806c487e7b0bbb => 190
	i64 16324796876805858114, ; 794: SkiaSharp.dll => 0xe28d5444586b6342 => 215
	i64 16337011941688632206, ; 795: System.Security.Principal.Windows.dll => 0xe2b8b9cdc3aa638e => 128
	i64 16361933716545543812, ; 796: Xamarin.AndroidX.ExifInterface.dll => 0xe3114406a52f1e84 => 271
	i64 16454459195343277943, ; 797: System.Net.NetworkInformation => 0xe459fb756d988f77 => 69
	i64 16496768397145114574, ; 798: Mono.Android.Export.dll => 0xe4f04b741db987ce => 170
	i64 16520416485536215268, ; 799: es\Microsoft.VisualStudio.Threading.resources => 0xe5444f43c1e4b0e4 => 397
	i64 16558262036769511634, ; 800: Microsoft.Extensions.Http => 0xe5cac397cf7b98d2 => 188
	i64 16565028646146589191, ; 801: System.ComponentModel.Composition.dll => 0xe5e2cdc9d3bcc207 => 218
	i64 16582434033142728747, ; 802: Microsoft.NET.StringTools.dll => 0xe620a3e548d2082b => 201
	i64 16583419235299384312, ; 803: ko\Microsoft.VisualStudio.Threading.resources => 0xe62423ee89665bf8 => 401
	i64 16584736615512946867, ; 804: Microsoft.VisualStudio.Utilities.dll => 0xe628d215051038b3 => 209
	i64 16589693266713801121, ; 805: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xe63a6e214f2a71a1 => 284
	i64 16621146507174665210, ; 806: Xamarin.AndroidX.ConstraintLayout => 0xe6aa2caf87dedbfa => 258
	i64 16643194905613199096, ; 807: System.Composition.Runtime => 0xe6f8819654aa66f8 => 222
	i64 16648892297579399389, ; 808: CommunityToolkit.Mvvm => 0xe70cbf55c4f508dd => 177
	i64 16649148416072044166, ; 809: Microsoft.Maui.Graphics => 0xe70da84600bb4e86 => 200
	i64 16677317093839702854, ; 810: Xamarin.AndroidX.Navigation.UI => 0xe771bb8960dd8b46 => 291
	i64 16702652415771857902, ; 811: System.ValueTuple => 0xe7cbbde0b0e6d3ee => 152
	i64 16709499819875633724, ; 812: System.IO.Compression.ZipFile => 0xe7e4118e32240a3c => 46
	i64 16737807731308835127, ; 813: System.Runtime.Intrinsics => 0xe848a3736f733137 => 109
	i64 16758309481308491337, ; 814: System.IO.FileSystem.DriveInfo => 0xe89179af15740e49 => 49
	i64 16762783179241323229, ; 815: System.Reflection.TypeExtensions => 0xe8a15e7d0d927add => 97
	i64 16765015072123548030, ; 816: System.Diagnostics.TextWriterTraceListener.dll => 0xe8a94c621bfe717e => 32
	i64 16800021547249663972, ; 817: Microsoft.ServiceHub.Framework => 0xe925aa963eb97fe4 => 202
	i64 16822611501064131242, ; 818: System.Data.DataSetExtensions => 0xe975ec07bb5412aa => 24
	i64 16827289987956835956, ; 819: cs\Microsoft.VisualStudio.Utilities.resources => 0xe9868b16d8a78674 => 408
	i64 16833383113903931215, ; 820: mscorlib => 0xe99c30c1484d7f4f => 167
	i64 16856067890322379635, ; 821: System.Data.Common.dll => 0xe9ecc87060889373 => 23
	i64 16870847574843270873, ; 822: de/Microsoft.ServiceHub.Resources.dll => 0xea214a7bd64b26d9 => 370
	i64 16890310621557459193, ; 823: System.Text.RegularExpressions.dll => 0xea66700587f088f9 => 139
	i64 16933958494752847024, ; 824: System.Net.WebProxy.dll => 0xeb018187f0f3b4b0 => 79
	i64 16942731696432749159, ; 825: sk\Microsoft.Maui.Controls.resources => 0xeb20acb622a01a67 => 347
	i64 16977952268158210142, ; 826: System.IO.Pipes.AccessControl => 0xeb9dcda2851b905e => 55
	i64 16989020923549080504, ; 827: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xebc52084add25bb8 => 284
	i64 16998075588627545693, ; 828: Xamarin.AndroidX.Navigation.Fragment => 0xebe54bb02d623e5d => 289
	i64 17008137082415910100, ; 829: System.Collections.NonGeneric => 0xec090a90408c8cd4 => 11
	i64 17018932226584424969, ; 830: pt-BR\Microsoft.VisualStudio.Threading.resources => 0xec2f64b09e033209 => 403
	i64 17024911836938395553, ; 831: Xamarin.AndroidX.Annotation.Experimental.dll => 0xec44a31d250e5fa1 => 241
	i64 17031351772568316411, ; 832: Xamarin.AndroidX.Navigation.Common.dll => 0xec5b843380a769fb => 288
	i64 17037200463775726619, ; 833: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xec704b8e0a78fc1b => 275
	i64 17062143951396181894, ; 834: System.ComponentModel.Primitives => 0xecc8e986518c9786 => 17
	i64 17089008752050867324, ; 835: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xed285aeb25888c7c => 354
	i64 17118171214553292978, ; 836: System.Threading.Channels => 0xed8ff6060fc420b2 => 140
	i64 17160917431135180830, ; 837: ru\Microsoft.VisualStudio.Utilities.resources => 0xee27d37b2e9f181e => 418
	i64 17180572832852637353, ; 838: es\StreamJsonRpc.resources => 0xee6da7f703e876a9 => 437
	i64 17187273293601214786, ; 839: System.ComponentModel.Annotations.dll => 0xee8575ff9aa89142 => 14
	i64 17201328579425343169, ; 840: System.ComponentModel.EventBasedAsync => 0xeeb76534d96c16c1 => 16
	i64 17202182880784296190, ; 841: System.Security.Cryptography.Encoding.dll => 0xeeba6e30627428fe => 123
	i64 17230721278011714856, ; 842: System.Private.Xml.Linq => 0xef1fd1b5c7a72d28 => 88
	i64 17234219099804750107, ; 843: System.Transactions.Local.dll => 0xef2c3ef5e11d511b => 150
	i64 17260702271250283638, ; 844: System.Data.Common => 0xef8a5543bba6bc76 => 23
	i64 17333249706306540043, ; 845: System.Diagnostics.Tracing.dll => 0xf08c12c5bb8b920b => 35
	i64 17338386382517543202, ; 846: System.Net.WebSockets.Client.dll => 0xf09e528d5c6da122 => 80
	i64 17342750010158924305, ; 847: hi\Microsoft.Maui.Controls.resources => 0xf0add33f97ecc211 => 332
	i64 17346290192329067203, ; 848: zh-Hant\Microsoft.VisualStudio.Threading.resources => 0xf0ba67067c9dcac3 => 407
	i64 17360349973592121190, ; 849: Xamarin.Google.Crypto.Tink.Android => 0xf0ec5a52686b9f66 => 314
	i64 17438153253682247751, ; 850: sk/Microsoft.Maui.Controls.resources.dll => 0xf200c3fe308d7847 => 347
	i64 17470386307322966175, ; 851: System.Threading.Timer => 0xf27347c8d0d5709f => 148
	i64 17490334362188023649, ; 852: pl\Microsoft.VisualStudio.Validation.resources => 0xf2ba266f45068761 => 429
	i64 17509662556995089465, ; 853: System.Net.WebSockets.dll => 0xf2fed1534ea67439 => 81
	i64 17514990004910432069, ; 854: fr\Microsoft.Maui.Controls.resources => 0xf311be9c6f341f45 => 330
	i64 17522591619082469157, ; 855: GoogleGson => 0xf32cc03d27a5bf25 => 178
	i64 17523946658117960076, ; 856: System.Security.Cryptography.ProtectedData.dll => 0xf33190a3c403c18c => 230
	i64 17590473451926037903, ; 857: Xamarin.Android.Glide => 0xf41dea67fcfda58f => 234
	i64 17623389608345532001, ; 858: pl\Microsoft.Maui.Controls.resources => 0xf492db79dfbef661 => 342
	i64 17627500474728259406, ; 859: System.Globalization => 0xf4a176498a351f4e => 43
	i64 17685921127322830888, ; 860: System.Diagnostics.Debug.dll => 0xf571038fafa74828 => 27
	i64 17702523067201099846, ; 861: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xf5abfef008ae1846 => 353
	i64 17704177640604968747, ; 862: Xamarin.AndroidX.Loader => 0xf5b1dfc36cac272b => 286
	i64 17710060891934109755, ; 863: Xamarin.AndroidX.Lifecycle.ViewModel => 0xf5c6c68c9e45303b => 283
	i64 17712670374920797664, ; 864: System.Runtime.InteropServices.dll => 0xf5d00bdc38bd3de0 => 108
	i64 17743407583038752114, ; 865: System.CodeDom.dll => 0xf63d3f302bff4572 => 217
	i64 17777860260071588075, ; 866: System.Runtime.Numerics.dll => 0xf6b7a5b72419c0eb => 111
	i64 17801122059615118823, ; 867: Microsoft.VisualStudio.Threading.dll => 0xf70a4a32e6baa5e7 => 208
	i64 17806780683907942013, ; 868: ja\Microsoft.VisualStudio.Composition.resources => 0xf71e64b0099ec67d => 387
	i64 17830918774018250459, ; 869: ja/Microsoft.ServiceHub.Resources.dll => 0xf7742627183396db => 374
	i64 17838668724098252521, ; 870: System.Buffers.dll => 0xf78faeb0f5bf3ee9 => 8
	i64 17849620690824033494, ; 871: cs\Microsoft.ServiceHub.Framework.resources => 0xf7b697726c0774d6 => 356
	i64 17852204549962223340, ; 872: ko/Microsoft.ServiceHub.Resources.dll => 0xf7bfc574021c4eec => 375
	i64 17859375626191382484, ; 873: fr/Microsoft.VisualStudio.Utilities.resources.dll => 0xf7d93f824d6827d4 => 412
	i64 17872609556538910715, ; 874: ru/Microsoft.ServiceHub.Framework.resources.dll => 0xf80843b2d2c0e7fb => 365
	i64 17891337867145587222, ; 875: Xamarin.Jetbrains.Annotations => 0xf84accff6fb52a16 => 317
	i64 17894845729534052403, ; 876: tr/Microsoft.VisualStudio.Composition.resources.dll => 0xf85743614b4b0833 => 392
	i64 17928294245072900555, ; 877: System.IO.Compression.FileSystem.dll => 0xf8ce18a0b24011cb => 45
	i64 17967933707255284319, ; 878: it/Microsoft.ServiceHub.Framework.resources.dll => 0xf95aec8230b8025f => 360
	i64 17992315986609351877, ; 879: System.Xml.XmlDocument.dll => 0xf9b18c0ffc6eacc5 => 162
	i64 18025913125965088385, ; 880: System.Threading => 0xfa28e87b91334681 => 149
	i64 18099568558057551825, ; 881: nl/Microsoft.Maui.Controls.resources.dll => 0xfb2e95b53ad977d1 => 341
	i64 18116111925905154859, ; 882: Xamarin.AndroidX.Arch.Core.Runtime => 0xfb695bd036cb632b => 246
	i64 18121036031235206392, ; 883: Xamarin.AndroidX.Navigation.Common => 0xfb7ada42d3d42cf8 => 288
	i64 18123834358115542242, ; 884: fr/StreamJsonRpc.resources.dll => 0xfb84cb53137f24e2 => 438
	i64 18146411883821974900, ; 885: System.Formats.Asn1.dll => 0xfbd50176eb22c574 => 39
	i64 18146811631844267958, ; 886: System.ComponentModel.EventBasedAsync.dll => 0xfbd66d08820117b6 => 16
	i64 18166800034629147097, ; 887: ru\StreamJsonRpc.resources => 0xfc1d7061319fd9d9 => 444
	i64 18171003490741864161, ; 888: MessagePack => 0xfc2c5f66960d46e1 => 179
	i64 18225059387460068507, ; 889: System.Threading.ThreadPool.dll => 0xfcec6af3cff4a49b => 147
	i64 18236439618683771511, ; 890: System.Threading.AccessControl.dll => 0xfd14d9365f819a77 => 232
	i64 18245806341561545090, ; 891: System.Collections.Concurrent.dll => 0xfd3620327d587182 => 9
	i64 18260797123374478311, ; 892: Xamarin.AndroidX.Emoji2 => 0xfd6b623bde35f3e7 => 269
	i64 18305135509493619199, ; 893: Xamarin.AndroidX.Navigation.Runtime.dll => 0xfe08e7c2d8c199ff => 290
	i64 18318849532986632368, ; 894: System.Security.dll => 0xfe39a097c37fa8b0 => 131
	i64 18324163916253801303, ; 895: it\Microsoft.Maui.Controls.resources => 0xfe4c81ff0a56ab57 => 336
	i64 18380184030268848184, ; 896: Xamarin.AndroidX.VersionedParcelable => 0xff1387fe3e7b7838 => 306
	i64 18439108438687598470 ; 897: System.Reflection.Metadata.dll => 0xffe4df6e2ee1c786 => 95
], align 8

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [898 x i32] [
	i32 405, ; 0
	i32 268, ; 1
	i32 205, ; 2
	i32 194, ; 3
	i32 172, ; 4
	i32 199, ; 5
	i32 414, ; 6
	i32 59, ; 7
	i32 378, ; 8
	i32 254, ; 9
	i32 152, ; 10
	i32 296, ; 11
	i32 255, ; 12
	i32 394, ; 13
	i32 299, ; 14
	i32 414, ; 15
	i32 262, ; 16
	i32 133, ; 17
	i32 193, ; 18
	i32 427, ; 19
	i32 209, ; 20
	i32 57, ; 21
	i32 298, ; 22
	i32 216, ; 23
	i32 358, ; 24
	i32 229, ; 25
	i32 329, ; 26
	i32 96, ; 27
	i32 420, ; 28
	i32 281, ; 29
	i32 130, ; 30
	i32 183, ; 31
	i32 401, ; 32
	i32 372, ; 33
	i32 227, ; 34
	i32 146, ; 35
	i32 386, ; 36
	i32 365, ; 37
	i32 256, ; 38
	i32 19, ; 39
	i32 433, ; 40
	i32 332, ; 41
	i32 267, ; 42
	i32 282, ; 43
	i32 251, ; 44
	i32 151, ; 45
	i32 105, ; 46
	i32 96, ; 47
	i32 311, ; 48
	i32 340, ; 49
	i32 434, ; 50
	i32 406, ; 51
	i32 37, ; 52
	i32 29, ; 53
	i32 245, ; 54
	i32 0, ; 55
	i32 289, ; 56
	i32 51, ; 57
	i32 116, ; 58
	i32 71, ; 59
	i32 196, ; 60
	i32 312, ; 61
	i32 66, ; 62
	i32 171, ; 63
	i32 367, ; 64
	i32 146, ; 65
	i32 338, ; 66
	i32 310, ; 67
	i32 244, ; 68
	i32 285, ; 69
	i32 275, ; 70
	i32 41, ; 71
	i32 90, ; 72
	i32 399, ; 73
	i32 82, ; 74
	i32 214, ; 75
	i32 67, ; 76
	i32 63, ; 77
	i32 87, ; 78
	i32 243, ; 79
	i32 107, ; 80
	i32 328, ; 81
	i32 296, ; 82
	i32 103, ; 83
	i32 36, ; 84
	i32 240, ; 85
	i32 350, ; 86
	i32 298, ; 87
	i32 419, ; 88
	i32 197, ; 89
	i32 177, ; 90
	i32 175, ; 91
	i32 350, ; 92
	i32 120, ; 93
	i32 283, ; 94
	i32 324, ; 95
	i32 342, ; 96
	i32 143, ; 97
	i32 142, ; 98
	i32 54, ; 99
	i32 36, ; 100
	i32 142, ; 101
	i32 371, ; 102
	i32 214, ; 103
	i32 422, ; 104
	i32 248, ; 105
	i32 237, ; 106
	i32 247, ; 107
	i32 191, ; 108
	i32 320, ; 109
	i32 267, ; 110
	i32 430, ; 111
	i32 9, ; 112
	i32 15, ; 113
	i32 346, ; 114
	i32 295, ; 115
	i32 52, ; 116
	i32 278, ; 117
	i32 388, ; 118
	i32 137, ; 119
	i32 102, ; 120
	i32 411, ; 121
	i32 415, ; 122
	i32 260, ; 123
	i32 305, ; 124
	i32 117, ; 125
	i32 238, ; 126
	i32 164, ; 127
	i32 349, ; 128
	i32 167, ; 129
	i32 68, ; 130
	i32 184, ; 131
	i32 324, ; 132
	i32 220, ; 133
	i32 81, ; 134
	i32 102, ; 135
	i32 300, ; 136
	i32 370, ; 137
	i32 118, ; 138
	i32 223, ; 139
	i32 224, ; 140
	i32 423, ; 141
	i32 329, ; 142
	i32 313, ; 143
	i32 407, ; 144
	i32 436, ; 145
	i32 79, ; 146
	i32 311, ; 147
	i32 115, ; 148
	i32 122, ; 149
	i32 49, ; 150
	i32 408, ; 151
	i32 212, ; 152
	i32 129, ; 153
	i32 276, ; 154
	i32 241, ; 155
	i32 83, ; 156
	i32 111, ; 157
	i32 76, ; 158
	i32 360, ; 159
	i32 321, ; 160
	i32 379, ; 161
	i32 402, ; 162
	i32 199, ; 163
	i32 54, ; 164
	i32 302, ; 165
	i32 181, ; 166
	i32 70, ; 167
	i32 301, ; 168
	i32 84, ; 169
	i32 364, ; 170
	i32 173, ; 171
	i32 344, ; 172
	i32 117, ; 173
	i32 182, ; 174
	i32 157, ; 175
	i32 181, ; 176
	i32 235, ; 177
	i32 425, ; 178
	i32 168, ; 179
	i32 294, ; 180
	i32 362, ; 181
	i32 268, ; 182
	i32 373, ; 183
	i32 226, ; 184
	i32 356, ; 185
	i32 189, ; 186
	i32 33, ; 187
	i32 179, ; 188
	i32 197, ; 189
	i32 415, ; 190
	i32 123, ; 191
	i32 73, ; 192
	i32 63, ; 193
	i32 221, ; 194
	i32 162, ; 195
	i32 114, ; 196
	i32 180, ; 197
	i32 89, ; 198
	i32 195, ; 199
	i32 355, ; 200
	i32 106, ; 201
	i32 19, ; 202
	i32 147, ; 203
	i32 119, ; 204
	i32 59, ; 205
	i32 262, ; 206
	i32 18, ; 207
	i32 204, ; 208
	i32 53, ; 209
	i32 379, ; 210
	i32 93, ; 211
	i32 225, ; 212
	i32 352, ; 213
	i32 56, ; 214
	i32 130, ; 215
	i32 203, ; 216
	i32 153, ; 217
	i32 42, ; 218
	i32 93, ; 219
	i32 384, ; 220
	i32 306, ; 221
	i32 425, ; 222
	i32 188, ; 223
	i32 51, ; 224
	i32 322, ; 225
	i32 389, ; 226
	i32 163, ; 227
	i32 447, ; 228
	i32 250, ; 229
	i32 14, ; 230
	i32 280, ; 231
	i32 238, ; 232
	i32 301, ; 233
	i32 37, ; 234
	i32 68, ; 235
	i32 110, ; 236
	i32 429, ; 237
	i32 399, ; 238
	i32 381, ; 239
	i32 358, ; 240
	i32 239, ; 241
	i32 395, ; 242
	i32 100, ; 243
	i32 100, ; 244
	i32 12, ; 245
	i32 12, ; 246
	i32 287, ; 247
	i32 26, ; 248
	i32 129, ; 249
	i32 77, ; 250
	i32 279, ; 251
	i32 380, ; 252
	i32 110, ; 253
	i32 357, ; 254
	i32 369, ; 255
	i32 305, ; 256
	i32 303, ; 257
	i32 107, ; 258
	i32 422, ; 259
	i32 418, ; 260
	i32 3, ; 261
	i32 27, ; 262
	i32 258, ; 263
	i32 158, ; 264
	i32 348, ; 265
	i32 233, ; 266
	i32 208, ; 267
	i32 22, ; 268
	i32 351, ; 269
	i32 50, ; 270
	i32 44, ; 271
	i32 127, ; 272
	i32 242, ; 273
	i32 60, ; 274
	i32 424, ; 275
	i32 120, ; 276
	i32 211, ; 277
	i32 443, ; 278
	i32 308, ; 279
	i32 271, ; 280
	i32 257, ; 281
	i32 4, ; 282
	i32 436, ; 283
	i32 277, ; 284
	i32 442, ; 285
	i32 297, ; 286
	i32 39, ; 287
	i32 125, ; 288
	i32 186, ; 289
	i32 345, ; 290
	i32 297, ; 291
	i32 345, ; 292
	i32 138, ; 293
	i32 150, ; 294
	i32 86, ; 295
	i32 91, ; 296
	i32 438, ; 297
	i32 281, ; 298
	i32 203, ; 299
	i32 448, ; 300
	i32 278, ; 301
	i32 435, ; 302
	i32 206, ; 303
	i32 333, ; 304
	i32 247, ; 305
	i32 264, ; 306
	i32 309, ; 307
	i32 192, ; 308
	i32 315, ; 309
	i32 279, ; 310
	i32 134, ; 311
	i32 215, ; 312
	i32 386, ; 313
	i32 395, ; 314
	i32 97, ; 315
	i32 445, ; 316
	i32 4, ; 317
	i32 341, ; 318
	i32 417, ; 319
	i32 202, ; 320
	i32 106, ; 321
	i32 344, ; 322
	i32 34, ; 323
	i32 409, ; 324
	i32 155, ; 325
	i32 159, ; 326
	i32 156, ; 327
	i32 83, ; 328
	i32 229, ; 329
	i32 273, ; 330
	i32 378, ; 331
	i32 144, ; 332
	i32 88, ; 333
	i32 20, ; 334
	i32 274, ; 335
	i32 52, ; 336
	i32 392, ; 337
	i32 219, ; 338
	i32 237, ; 339
	i32 348, ; 340
	i32 62, ; 341
	i32 377, ; 342
	i32 55, ; 343
	i32 5, ; 344
	i32 98, ; 345
	i32 236, ; 346
	i32 391, ; 347
	i32 18, ; 348
	i32 156, ; 349
	i32 85, ; 350
	i32 430, ; 351
	i32 30, ; 352
	i32 361, ; 353
	i32 46, ; 354
	i32 385, ; 355
	i32 65, ; 356
	i32 426, ; 357
	i32 383, ; 358
	i32 67, ; 359
	i32 339, ; 360
	i32 173, ; 361
	i32 282, ; 362
	i32 2, ; 363
	i32 318, ; 364
	i32 312, ; 365
	i32 48, ; 366
	i32 25, ; 367
	i32 244, ; 368
	i32 385, ; 369
	i32 187, ; 370
	i32 400, ; 371
	i32 381, ; 372
	i32 166, ; 373
	i32 109, ; 374
	i32 444, ; 375
	i32 218, ; 376
	i32 13, ; 377
	i32 276, ; 378
	i32 64, ; 379
	i32 28, ; 380
	i32 24, ; 381
	i32 94, ; 382
	i32 169, ; 383
	i32 13, ; 384
	i32 319, ; 385
	i32 442, ; 386
	i32 396, ; 387
	i32 0, ; 388
	i32 200, ; 389
	i32 30, ; 390
	i32 104, ; 391
	i32 232, ; 392
	i32 15, ; 393
	i32 252, ; 394
	i32 127, ; 395
	i32 259, ; 396
	i32 291, ; 397
	i32 390, ; 398
	i32 92, ; 399
	i32 280, ; 400
	i32 210, ; 401
	i32 231, ; 402
	i32 10, ; 403
	i32 362, ; 404
	i32 87, ; 405
	i32 413, ; 406
	i32 421, ; 407
	i32 400, ; 408
	i32 270, ; 409
	i32 174, ; 410
	i32 303, ; 411
	i32 410, ; 412
	i32 343, ; 413
	i32 72, ; 414
	i32 169, ; 415
	i32 249, ; 416
	i32 2, ; 417
	i32 290, ; 418
	i32 6, ; 419
	i32 343, ; 420
	i32 45, ; 421
	i32 28, ; 422
	i32 359, ; 423
	i32 426, ; 424
	i32 186, ; 425
	i32 363, ; 426
	i32 397, ; 427
	i32 159, ; 428
	i32 293, ; 429
	i32 113, ; 430
	i32 357, ; 431
	i32 320, ; 432
	i32 353, ; 433
	i32 249, ; 434
	i32 122, ; 435
	i32 439, ; 436
	i32 308, ; 437
	i32 243, ; 438
	i32 359, ; 439
	i32 160, ; 440
	i32 132, ; 441
	i32 314, ; 442
	i32 58, ; 443
	i32 423, ; 444
	i32 139, ; 445
	i32 84, ; 446
	i32 31, ; 447
	i32 260, ; 448
	i32 439, ; 449
	i32 11, ; 450
	i32 213, ; 451
	i32 310, ; 452
	i32 172, ; 453
	i32 368, ; 454
	i32 257, ; 455
	i32 368, ; 456
	i32 151, ; 457
	i32 95, ; 458
	i32 427, ; 459
	i32 270, ; 460
	i32 61, ; 461
	i32 198, ; 462
	i32 217, ; 463
	i32 367, ; 464
	i32 158, ; 465
	i32 207, ; 466
	i32 328, ; 467
	i32 191, ; 468
	i32 65, ; 469
	i32 89, ; 470
	i32 80, ; 471
	i32 48, ; 472
	i32 196, ; 473
	i32 144, ; 474
	i32 325, ; 475
	i32 210, ; 476
	i32 183, ; 477
	i32 388, ; 478
	i32 264, ; 479
	i32 75, ; 480
	i32 92, ; 481
	i32 317, ; 482
	i32 136, ; 483
	i32 91, ; 484
	i32 302, ; 485
	i32 321, ; 486
	i32 394, ; 487
	i32 261, ; 488
	i32 409, ; 489
	i32 220, ; 490
	i32 387, ; 491
	i32 323, ; 492
	i32 113, ; 493
	i32 43, ; 494
	i32 160, ; 495
	i32 5, ; 496
	i32 104, ; 497
	i32 71, ; 498
	i32 193, ; 499
	i32 61, ; 500
	i32 40, ; 501
	i32 416, ; 502
	i32 204, ; 503
	i32 245, ; 504
	i32 174, ; 505
	i32 154, ; 506
	i32 57, ; 507
	i32 35, ; 508
	i32 421, ; 509
	i32 190, ; 510
	i32 198, ; 511
	i32 242, ; 512
	i32 22, ; 513
	i32 164, ; 514
	i32 212, ; 515
	i32 315, ; 516
	i32 180, ; 517
	i32 375, ; 518
	i32 334, ; 519
	i32 374, ; 520
	i32 384, ; 521
	i32 313, ; 522
	i32 391, ; 523
	i32 307, ; 524
	i32 141, ; 525
	i32 337, ; 526
	i32 192, ; 527
	i32 90, ; 528
	i32 255, ; 529
	i32 148, ; 530
	i32 263, ; 531
	i32 224, ; 532
	i32 163, ; 533
	i32 434, ; 534
	i32 292, ; 535
	i32 373, ; 536
	i32 369, ; 537
	i32 7, ; 538
	i32 170, ; 539
	i32 32, ; 540
	i32 404, ; 541
	i32 398, ; 542
	i32 108, ; 543
	i32 273, ; 544
	i32 228, ; 545
	i32 335, ; 546
	i32 307, ; 547
	i32 189, ; 548
	i32 240, ; 549
	i32 300, ; 550
	i32 412, ; 551
	i32 168, ; 552
	i32 383, ; 553
	i32 274, ; 554
	i32 141, ; 555
	i32 331, ; 556
	i32 60, ; 557
	i32 145, ; 558
	i32 406, ; 559
	i32 175, ; 560
	i32 441, ; 561
	i32 366, ; 562
	i32 82, ; 563
	i32 416, ; 564
	i32 230, ; 565
	i32 75, ; 566
	i32 131, ; 567
	i32 26, ; 568
	i32 8, ; 569
	i32 94, ; 570
	i32 417, ; 571
	i32 304, ; 572
	i32 138, ; 573
	i32 234, ; 574
	i32 114, ; 575
	i32 10, ; 576
	i32 228, ; 577
	i32 105, ; 578
	i32 176, ; 579
	i32 382, ; 580
	i32 20, ; 581
	i32 272, ; 582
	i32 286, ; 583
	i32 372, ; 584
	i32 448, ; 585
	i32 447, ; 586
	i32 266, ; 587
	i32 34, ; 588
	i32 253, ; 589
	i32 47, ; 590
	i32 431, ; 591
	i32 396, ; 592
	i32 336, ; 593
	i32 31, ; 594
	i32 254, ; 595
	i32 58, ; 596
	i32 135, ; 597
	i32 115, ; 598
	i32 201, ; 599
	i32 309, ; 600
	i32 250, ; 601
	i32 435, ; 602
	i32 349, ; 603
	i32 56, ; 604
	i32 194, ; 605
	i32 7, ; 606
	i32 207, ; 607
	i32 78, ; 608
	i32 411, ; 609
	i32 227, ; 610
	i32 265, ; 611
	i32 393, ; 612
	i32 382, ; 613
	i32 112, ; 614
	i32 445, ; 615
	i32 223, ; 616
	i32 269, ; 617
	i32 398, ; 618
	i32 231, ; 619
	i32 103, ; 620
	i32 323, ; 621
	i32 337, ; 622
	i32 364, ; 623
	i32 171, ; 624
	i32 116, ; 625
	i32 331, ; 626
	i32 304, ; 627
	i32 259, ; 628
	i32 77, ; 629
	i32 316, ; 630
	i32 86, ; 631
	i32 318, ; 632
	i32 351, ; 633
	i32 246, ; 634
	i32 219, ; 635
	i32 352, ; 636
	i32 335, ; 637
	i32 294, ; 638
	i32 161, ; 639
	i32 3, ; 640
	i32 265, ; 641
	i32 25, ; 642
	i32 239, ; 643
	i32 33, ; 644
	i32 211, ; 645
	i32 118, ; 646
	i32 38, ; 647
	i32 17, ; 648
	i32 330, ; 649
	i32 53, ; 650
	i32 333, ; 651
	i32 371, ; 652
	i32 432, ; 653
	i32 21, ; 654
	i32 233, ; 655
	i32 124, ; 656
	i32 155, ; 657
	i32 272, ; 658
	i32 361, ; 659
	i32 419, ; 660
	i32 132, ; 661
	i32 325, ; 662
	i32 253, ; 663
	i32 149, ; 664
	i32 380, ; 665
	i32 235, ; 666
	i32 121, ; 667
	i32 29, ; 668
	i32 133, ; 669
	i32 101, ; 670
	i32 135, ; 671
	i32 292, ; 672
	i32 154, ; 673
	i32 248, ; 674
	i32 98, ; 675
	i32 126, ; 676
	i32 403, ; 677
	i32 236, ; 678
	i32 70, ; 679
	i32 432, ; 680
	i32 213, ; 681
	i32 443, ; 682
	i32 424, ; 683
	i32 73, ; 684
	i32 346, ; 685
	i32 277, ; 686
	i32 295, ; 687
	i32 327, ; 688
	i32 251, ; 689
	i32 137, ; 690
	i32 176, ; 691
	i32 125, ; 692
	i32 72, ; 693
	i32 393, ; 694
	i32 112, ; 695
	i32 440, ; 696
	i32 287, ; 697
	i32 205, ; 698
	i32 184, ; 699
	i32 153, ; 700
	i32 338, ; 701
	i32 354, ; 702
	i32 446, ; 703
	i32 389, ; 704
	i32 316, ; 705
	i32 420, ; 706
	i32 405, ; 707
	i32 119, ; 708
	i32 206, ; 709
	i32 263, ; 710
	i32 178, ; 711
	i32 355, ; 712
	i32 322, ; 713
	i32 128, ; 714
	i32 134, ; 715
	i32 185, ; 716
	i32 78, ; 717
	i32 47, ; 718
	i32 266, ; 719
	i32 74, ; 720
	i32 437, ; 721
	i32 441, ; 722
	i32 64, ; 723
	i32 410, ; 724
	i32 187, ; 725
	i32 99, ; 726
	i32 85, ; 727
	i32 402, ; 728
	i32 339, ; 729
	i32 44, ; 730
	i32 62, ; 731
	i32 293, ; 732
	i32 440, ; 733
	i32 182, ; 734
	i32 38, ; 735
	i32 363, ; 736
	i32 446, ; 737
	i32 41, ; 738
	i32 256, ; 739
	i32 319, ; 740
	i32 161, ; 741
	i32 221, ; 742
	i32 99, ; 743
	i32 222, ; 744
	i32 261, ; 745
	i32 185, ; 746
	i32 376, ; 747
	i32 225, ; 748
	i32 377, ; 749
	i32 136, ; 750
	i32 21, ; 751
	i32 66, ; 752
	i32 326, ; 753
	i32 126, ; 754
	i32 76, ; 755
	i32 431, ; 756
	i32 285, ; 757
	i32 165, ; 758
	i32 157, ; 759
	i32 326, ; 760
	i32 413, ; 761
	i32 6, ; 762
	i32 334, ; 763
	i32 252, ; 764
	i32 404, ; 765
	i32 50, ; 766
	i32 299, ; 767
	i32 327, ; 768
	i32 145, ; 769
	i32 140, ; 770
	i32 1, ; 771
	i32 101, ; 772
	i32 433, ; 773
	i32 195, ; 774
	i32 124, ; 775
	i32 121, ; 776
	i32 428, ; 777
	i32 143, ; 778
	i32 40, ; 779
	i32 69, ; 780
	i32 42, ; 781
	i32 366, ; 782
	i32 428, ; 783
	i32 1, ; 784
	i32 376, ; 785
	i32 165, ; 786
	i32 216, ; 787
	i32 226, ; 788
	i32 74, ; 789
	i32 390, ; 790
	i32 340, ; 791
	i32 166, ; 792
	i32 190, ; 793
	i32 215, ; 794
	i32 128, ; 795
	i32 271, ; 796
	i32 69, ; 797
	i32 170, ; 798
	i32 397, ; 799
	i32 188, ; 800
	i32 218, ; 801
	i32 201, ; 802
	i32 401, ; 803
	i32 209, ; 804
	i32 284, ; 805
	i32 258, ; 806
	i32 222, ; 807
	i32 177, ; 808
	i32 200, ; 809
	i32 291, ; 810
	i32 152, ; 811
	i32 46, ; 812
	i32 109, ; 813
	i32 49, ; 814
	i32 97, ; 815
	i32 32, ; 816
	i32 202, ; 817
	i32 24, ; 818
	i32 408, ; 819
	i32 167, ; 820
	i32 23, ; 821
	i32 370, ; 822
	i32 139, ; 823
	i32 79, ; 824
	i32 347, ; 825
	i32 55, ; 826
	i32 284, ; 827
	i32 289, ; 828
	i32 11, ; 829
	i32 403, ; 830
	i32 241, ; 831
	i32 288, ; 832
	i32 275, ; 833
	i32 17, ; 834
	i32 354, ; 835
	i32 140, ; 836
	i32 418, ; 837
	i32 437, ; 838
	i32 14, ; 839
	i32 16, ; 840
	i32 123, ; 841
	i32 88, ; 842
	i32 150, ; 843
	i32 23, ; 844
	i32 35, ; 845
	i32 80, ; 846
	i32 332, ; 847
	i32 407, ; 848
	i32 314, ; 849
	i32 347, ; 850
	i32 148, ; 851
	i32 429, ; 852
	i32 81, ; 853
	i32 330, ; 854
	i32 178, ; 855
	i32 230, ; 856
	i32 234, ; 857
	i32 342, ; 858
	i32 43, ; 859
	i32 27, ; 860
	i32 353, ; 861
	i32 286, ; 862
	i32 283, ; 863
	i32 108, ; 864
	i32 217, ; 865
	i32 111, ; 866
	i32 208, ; 867
	i32 387, ; 868
	i32 374, ; 869
	i32 8, ; 870
	i32 356, ; 871
	i32 375, ; 872
	i32 412, ; 873
	i32 365, ; 874
	i32 317, ; 875
	i32 392, ; 876
	i32 45, ; 877
	i32 360, ; 878
	i32 162, ; 879
	i32 149, ; 880
	i32 341, ; 881
	i32 246, ; 882
	i32 288, ; 883
	i32 438, ; 884
	i32 39, ; 885
	i32 16, ; 886
	i32 444, ; 887
	i32 179, ; 888
	i32 147, ; 889
	i32 232, ; 890
	i32 9, ; 891
	i32 269, ; 892
	i32 290, ; 893
	i32 131, ; 894
	i32 336, ; 895
	i32 306, ; 896
	i32 95 ; 897
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 8

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 8

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 8

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 8, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" }

; Metadata
!llvm.module.flags = !{!0, !1, !7, !8, !9, !10}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.2xx @ 96b6bb65e8736e45180905177aa343f0e1854ea3"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"branch-target-enforcement", i32 0}
!8 = !{i32 1, !"sign-return-address", i32 0}
!9 = !{i32 1, !"sign-return-address-all", i32 0}
!10 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
