# Cheat Sheet

# Câu hỏi

### Tôi muốn kiểm soát thứ tự chạy giữa 2 hàm Awake() trong Unity thì làm như thế nào?

Trong Unity, việc kiểm soát chính xác thứ tự chạy của phương thức `Awake` giữa các đối tượng khác nhau là không thể dựa vào cơ chế mặc định của Unity. Tuy nhiên, có một số cách tiếp cận khác nhau bạn có thể sử dụng để đảm bảo rằng một hành động hoặc phương thức xảy ra sau khi một hành động hoặc phương thức khác đã hoàn tất.

### Phương pháp 1: Sử dụng Flag và `Start`

Một cách để làm điều này là sử dụng một biến flag để kiểm tra xem phương thức trong `Awake` của Đối tượng A đã chạy xong chưa. Sau đó, bạn có thể chờ đến khi flag này được thiết lập trước khi thực hiện hành động trong Đối tượng B.

**Ví dụ:**

Trong script của Đối tượng A:

```csharp
public class ObjectA : MonoBehaviour
{
    public static bool IsInitialized = false;

    void Awake()
    {
        // Thực hiện hành động...
        IsInitialized = true;
    }
}
```

Trong script của Đối tượng B:

```csharp
public class ObjectB : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitForInitialization());
    }

    IEnumerator WaitForInitialization()
    {
        // Chờ đợi Đối tượng A hoàn tất
        yield return new WaitUntil(() => ObjectA.IsInitialized);

        // Tiếp tục với phương thức B
        MethodB();
    }

    void MethodB()
    {
        // Phương thức B
    }
}
```

### Phương pháp 2: Sử dụng Một Manager hoặc Controller

Một cách khác là sử dụng một đối tượng quản lý hoặc controller cấp cao để điều khiển thứ tự khởi tạo.

**Ví dụ:**

```csharp
public class InitializationManager : MonoBehaviour
{
    [SerializeField] private ObjectA objectA;
    [SerializeField] private ObjectB objectB;

    void Awake()
    {
        objectA.Initialize();
        objectB.Initialize();
    }
}

```

Trong ví dụ này, `InitializationManager` gọi các phương thức khởi tạo của Đối tượng A và B theo thứ tự mong muốn.

### Lưu ý

Cả hai phương pháp trên đều yêu cầu bạn thiết kế code sao cho không phụ thuộc quá mức vào thứ tự khởi tạo. Điều này giúp làm cho code của bạn linh hoạt và dễ bảo trì hơn.

### Khác nhau giữa Coroutine và async/await

Coroutines và async/await trong lập trình (đặc biệt là trong Unity) là hai cách tiếp cận khác nhau để xử lý các tác vụ không đồng bộ và thời gian. Mỗi cách có ưu và nhược điểm riêng, và được sử dụng trong các tình huống khác nhau. Dưới đây là sự khác biệt cơ bản giữa chúng:

### Coroutines

1. **Định Nghĩa:**
    - Coroutines trong Unity là một cách để thực hiện các tác vụ qua nhiều khung hình mà không chặn tiến trình của game.
2. **Hoạt Động:**
    - Sử dụng `yield` để tạm dừng thực thi của coroutine trong một hoặc nhiều khung hình.
3. **Ứng Dụng:**
    - Thường được sử dụng trong Unity để xử lý thời gian (ví dụ: đợi trước khi thực hiện hành động, lặp qua một thời gian cố định).
4. **Kiểm Soát Thời Gian:**
    - Cung cấp nhiều lệnh `yield` khác nhau cho việc kiểm soát thời gian (ví dụ: `WaitForSeconds`, `WaitUntil`).
5. **Không Phải Là Một Tính Năng C# Chính Thống:**
    - Là một phần của Unity Engine và không phải là một phần của ngôn ngữ C# mặc định.

### Async/Await

1. **Định Nghĩa:**
    - Async/await là một cách tiếp cận trong C# để thực hiện các tác vụ không đồng bộ, cho phép viết code trông giống như đồng bộ nhưng thực chất là không đồng bộ.
2. **Hoạt Động:**
    - Cho phép một phương thức trả về `Task` hoặc `Task<T>` và sử dụng `await` để tạm dừng thực thi mà không chặn thread chính.
3. **Ứng Dụng:**
    - Phổ biến trong các tác vụ như truy cập mạng, đọc/ghi file, và các tác vụ có thể mất thời gian mà không liên quan đến thời gian thực của game.
4. **Kiểm Soát Thời Gian:**
    - Không cung cấp kiểm soát trực tiếp trên thời gian như coroutines, tập trung hơn vào việc xử lý tác vụ không đồng bộ.
5. **Là Một Tính Năng C# Chính Thống:**
    - Là một phần của ngôn ngữ C# và có thể sử dụng trong bất kỳ ứng dụng C# nào.

### Khi Nào Sử Dụng?

- **Coroutines:** Tốt cho các tác vụ liên quan đến thời gian trong game, như animations, chờ đợi trước khi thực hiện hành động, hoặc thực hiện một hành động theo khoảng thời gian định trước.
- **Async/Await:** Hiệu quả cho các tác vụ không đồng bộ không phụ thuộc vào thời gian thực của game, như truy cập mạng, giao tiếp với cơ sở dữ liệu, hoặc đọc/ghi file.

Sự lựa chọn giữa hai cách tiếp cận này phụ thuộc vào yêu cầu cụ thể của tác vụ bạn đang thực hiện và môi trường lập trình mà bạn làm việc (Unity hoặc C# chung).

### Thread Pool là gì?

Thread pool trong lập trình máy tính là một tập hợp các luồng (threads) đã được tạo trước và duy trì sẵn sàng để thực thi các nhiệm vụ. Sử dụng thread pool có một số lợi ích so với việc tạo mới luồng mỗi khi cần thực hiện một nhiệm vụ. Dưới đây là các điểm chính về thread pool:

### Khái Niệm

- **Tạo Trước và Quản Lý Luồng:** Thread pool tạo trước một số lượng nhất định luồng và quản lý chúng. Khi một nhiệm vụ mới cần được thực hiện, nó sẽ được giao cho một luồng trong pool thay vì tạo một luồng mới.
- **Tái Sử Dụng Luồng:** Các luồng trong pool có thể tái sử dụng cho nhiều nhiệm vụ khác nhau, giảm thiểu chi phí tạo và hủy luồng.

### Lợi Ích

1. **Hiệu Suất Cao:** Việc tạo và hủy luồng là tốn kém về tài nguyên hệ thống. Thread pool giúp giảm thiểu chi phí này bằng cách tái sử dụng luồng.
2. **Giảm Thiểu Độ Trễ:** Khi có nhiệm vụ mới, việc sử dụng một luồng sẵn có từ pool có thể giảm thiểu độ trễ so với việc phải tạo mới luồng.
3. **Quản Lý Tài Nguyên:** Thread pool giúp quản lý số lượng luồng đang hoạt động, tránh tình trạng quá tải cho hệ thống do quá nhiều luồng đồng thời.

### Ứng Dụng

- Thread pool thường được sử dụng trong các ứng dụng server, xử lý yêu cầu đồng thời từ nhiều người dùng.
- Trong lập trình .NET, `ThreadPool` class cung cấp cơ chế quản lý thread pool.
- Trong Java, các executor service như `ThreadPoolExecutor` và `ForkJoinPool` cung cấp cơ chế tương tự.

### Khi Nào Sử Dụng

- Sử dụng thread pool khi bạn cần thực hiện nhiều nhiệm vụ nhỏ và độc lập, đặc biệt là trong môi trường đa luồng.
- Không khuyến khích sử dụng thread pool cho các nhiệm vụ lâu dài hoặc chặn luồng, vì điều này có thể làm cạn kiệt các luồng trong pool và làm chậm toàn bộ hệ thống.

Trong Unity, việc sử dụng thread pool không phải là một tính năng được hỗ trợ một cách trực tiếp thông qua các công cụ hoặc API của Unity. Tuy nhiên, bạn có thể sử dụng các tính năng của .NET, chẳng hạn như `ThreadPool`, để thực hiện các tác vụ không đồng bộ trong game của bạn. Dưới đây là một ví dụ về cách sử dụng `ThreadPool` trong một project Unity:

### Ví dụ: Sử dụng ThreadPool trong Unity

Trước hết, bạn cần đảm bảo rằng project của bạn đang sử dụng .NET 4.x hoặc mới hơn.

```csharp
using System.Threading;
using UnityEngine;

public class ThreadPoolExample : MonoBehaviour
{
    void Start()
    {
        // Gửi một nhiệm vụ tới thread pool
        ThreadPool.QueueUserWorkItem(DoWork);
    }

    void DoWork(object state)
    {
        // Thực hiện một số tác vụ nặng
        Debug.Log("Bắt đầu xử lý tác vụ trên thread pool...");

        // Giả sử đây là một tác vụ nặng
        Thread.Sleep(2000);

        // Gửi kết quả về main thread bằng cách sử dụng 'UnityMainThreadDispatcher' hoặc tương tự
        UnityMainThreadDispatcher.Instance().Enqueue(() => Debug.Log("Tác vụ hoàn thành."));
    }
}

```

### Lưu ý quan trọng:

1. **MainThreadDispatcher:** `UnityMainThreadDispatcher` là một utility không tồn tại sẵn trong Unity. Bạn cần tạo nó để gửi kết quả từ thread pool về main thread, vì Unity không cho phép thao tác với các đối tượng của nó từ các thread khác ngoài main thread.
2. **Thận Trọng với Đa Luồng:** Sử dụng thread pool trong Unity đòi hỏi sự cẩn thận vì Unity không đảm bảo an toàn khi sử dụng đa luồng. Hãy chắc chắn rằng bạn không thực hiện các thao tác liên quan đến Unity API trực tiếp từ các thread không phải là main thread.
3. **Tác Vụ Nặng:** `ThreadPool` thích hợp cho việc xử lý các tác vụ nặng và không đồng bộ như tính toán, xử lý file, hoặc các hoạt động không liên quan trực tiếp đến các đối tượng game.

Sử dụng `ThreadPool` trong Unity có thể giúp tối ưu hóa hiệu suất của các tác vụ nặng, nhưng nó đòi hỏi sự hiểu biết sâu sắc về đa luồng và cách Unity xử lý các thread.

### UnityMainThreadDispatcher là gì?

UnityMainThreadDispatcher là một công cụ hoặc mẫu thiết kế phổ biến trong phát triển game với Unity, dùng để quản lý việc thực thi các tác vụ trên main thread (chủ yếu là để tương tác với Unity API). Đây không phải là một phần của Unity Engine mặc định, nhưng là một lớp mà các nhà phát triển thường tự tạo ra hoặc sử dụng từ các nguồn thứ ba.

### Tại Sao Cần UnityMainThreadDispatcher?

Trong Unity, hầu hết các tác vụ liên quan đến Unity API (như thay đổi trạng thái của GameObject, cập nhật UI, v.v.) phải được thực hiện trên main thread. Khi làm việc với đa luồng hoặc các hàm không đồng bộ (như sử dụng `async/await` hoặc callbacks từ các thư viện bên ngoài), bạn thường cần một cách để chuyển việc thực thi các tác vụ này trở lại main thread.

### Cách Hoạt Động của UnityMainThreadDispatcher

UnityMainThreadDispatcher thường được triển khai như một Singleton, đảm bảo chỉ có một instance duy nhất trong một cảnh. Nó cho phép các script khác trong game "đăng ký" các hành động để được thực thi trên main thread. Thông thường, nó có một hàng đợi (queue) các hành động, và trong mỗi khung hình (ví dụ trong `Update()`), nó sẽ xử lý và thực thi các hành động trong hàng đợi này.

### Ví dụ Mô Phỏng Cách Triển Khai

Dưới đây là một ví dụ đơn giản về cách bạn có thể triển khai UnityMainThreadDispatcher:

```csharp
using UnityEngine;
using System.Collections.Generic;
using System;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance = null;
    private readonly Queue<Action> queue = new Queue<Action>();

    public static UnityMainThreadDispatcher Instance()
    {
        if (!instance)
        {
            instance = FindObjectOfType<UnityMainThreadDispatcher>();
            if (!instance)
            {
                instance = new GameObject("UnityMainThreadDispatcher").AddComponent<UnityMainThreadDispatcher>();
            }
        }
        return instance;
    }

    public void Enqueue(Action action)
    {
        queue.Enqueue(action);
    }

    void Update()
    {
        while (queue.Count > 0)
        {
            queue.Dequeue().Invoke();
        }
    }
}

```

Trong triển khai này:

- `UnityMainThreadDispatcher` là một Singleton.
- Các hành động từ các thread khác được thêm vào hàng đợi thông qua phương thức `Enqueue`.
- Trong mỗi khung hình, tất cả các hành động trong hàng đợi được thực thi trong phương thức `Update`.

### Sử Dụng

Để sử dụng UnityMainThreadDispatcher, các script khác có thể gọi `UnityMainThreadDispatcher.Instance().Enqueue(() => { /* code */ });` để đặt một hành động cụ thể để thực thi trên main thread.

Lưu ý rằng, việc sử dụng UnityMainThreadDispatcher đòi hỏi phải hiểu rõ về đa luồng và cách Unity xử lý các tác vụ trong main thread. Đây là một công cụ hữu ích trong nhiều tình huống, đặc biệt khi làm việc với đa luồng hoặc các hàm không đồng bộ ngoài Unity API.

### UnityWebRequest là gì?

`UnityWebRequest` là một lớp trong Unity Engine được thiết kế để tương tác với các tài nguyên trên mạng. Nó cung cấp một giao diện để gửi và nhận dữ liệu từ Internet. `UnityWebRequest` được tối ưu hóa cho việc sử dụng trong môi trường game, hỗ trợ các tác vụ không đồng bộ, và có khả năng xử lý nhiều loại dữ liệu khác nhau, từ văn bản đơn giản đến hình ảnh và video.

### Tính Năng Chính của `UnityWebRequest`:

1. **Gửi Yêu Cầu HTTP/HTTPS:** Cho phép tạo các yêu cầu HTTP để tương tác với web servers, REST APIs, và các dịch vụ trực tuyến khác.
2. **Hỗ Trợ Tác Vụ Không Đồng Bộ:** Có thể sử dụng cùng với `yield return` trong coroutines của Unity để thực hiện các yêu cầu mà không chặn main thread, giúp trò chơi hoạt động mượt mà hơn.
3. **Xử Lý Đa Dạng Dữ Liệu:** Có khả năng xử lý và tải xuống nhiều loại dữ liệu, từ văn bản (JSON, XML, HTML), hình ảnh, đến file nhị phân.
4. **Quản Lý Tải Lên và Tải Xuống Dữ Liệu:** Cho phép tải lên dữ liệu (như hình ảnh, video, hoặc dữ liệu khác) và tải xuống dữ liệu từ server.
5. **Tùy Chỉnh Headers và Method:** Cho phép tùy chỉnh tiêu đề yêu cầu (headers) và phương thức (GET, POST, PUT, DELETE, v.v).

### ****Ví dụ: Sử dụng UnityWebRequest trong Async/Await****

```csharp
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class UnityWebRequestAsyncExample : MonoBehaviour
{
    async void Start()
    {
        string url = "http://example.com";
        string result = await FetchDataFromUrl(url);
        Debug.Log(result);
    }

    async Task<string> FetchDataFromUrl(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Gửi yêu cầu và chờ cho đến khi hoàn thành
            await webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                // Trả về lỗi nếu có
                return webRequest.error;
            }
            else
            {
                // Trả về nội dung nhận được
                return webRequest.downloadHandler.text;
            }
        }
    }
}
```

### **Giải Thích:**

- **Async/Await:** Phương thức **`Start`** sử dụng từ khóa **`async`** và **`await`** để gọi **`FetchDataFromUrl`** một cách không đồng bộ.
- **Đợi Yêu Cầu Hoàn Thành:** Trong **`FetchDataFromUrl`**, **`await webRequest.SendWebRequest()`** tạm dừng thực thi của phương thức cho đến khi yêu cầu hoàn thành, nhưng không chặn luồng chính của game.
- **Xử Lý Kết Quả:** Sau khi yêu cầu hoàn thành, phương thức kiểm tra lỗi và trả về nội dung phản hồi hoặc thông báo lỗi.
- **Không Chặn Luồng Chính:** Việc sử dụng async/await cho phép việc tải dữ liệu từ mạng diễn ra mà không gây gián đoạn tới luồng chính, giữ cho game chạy mượt mà.

### **Lưu ý:**

- **Unity Version:** Đảm bảo rằng bạn đang sử dụng Unity phiên bản hỗ trợ C# 6.0 trở lên để sử dụng async/await.
- **Xử lý Ngoại Lệ:** Cần có cơ chế xử lý ngoại lệ trong trường hợp yêu cầu thất bại.

Đây là một ví dụ về cách hiệu quả để sử dụng **`UnityWebRequest`** trong các tác vụ không đồng bộ, cho phép bạn thực hiện yêu cầu mạng trong Unity mà không ảnh hưởng đến hiệu suất hoạt động của game.

### Cách tạo thư mục (folder) bằng code trong Unity

Để tạo một thư mục mới trong Unity thông qua script C#, bạn có thể sử dụng các chức năng của .NET Framework. Dưới đây là một ví dụ về cách tạo một thư mục có tên "newLevel" trong thư mục "Assets" của dự án Unity của bạn:

```csharp
using System.IO;
using UnityEngine;

public class CreateDirectoryExample : MonoBehaviour
{
    private string folderName = "newLevel";

    void Start()
    {
        CreateDirectory(folderName);
    }

    private void CreateDirectory(string folderName)
    {
        // Xác định đường dẫn đầy đủ tới thư mục mới
        string folderPath = Path.Combine(Application.dataPath, folderName);

        // Kiểm tra xem thư mục đã tồn tại chưa
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("Thư mục mới đã được tạo: " + folderPath);

            // Yêu cầu Unity cập nhật và hiển thị thư mục mới trong Editor
            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            #endif
        }
        else
        {
            Debug.Log("Thư mục đã tồn tại: " + folderPath);
        }
    }
}

```

Trong script này:

- `Application.dataPath` trả về đường dẫn tới thư mục "Assets" trong dự án Unity của bạn.
- `Directory.Exists` kiểm tra xem thư mục với đường dẫn chỉ định đã tồn tại hay chưa.
- `Directory.CreateDirectory` tạo thư mục mới nếu nó chưa tồn tại.
- `UnityEditor.AssetDatabase.Refresh` được gọi để cập nhật Unity Editor và làm cho thư mục mới hiển thị trong cửa sổ Project. Dòng này chỉ cần thiết khi bạn muốn kết quả cập nhật ngay lập tức trong Unity Editor và chỉ được sử dụng trong Unity Editor, không trong build cuối cùng của game.

Lưu ý rằng việc tạo thư mục và làm việc với hệ thống tệp nên chỉ được thực hiện trong quá trình phát triển và không thường xuyên được sử dụng trong game hoạt động.

### Cách tạo ScriptableObject bằng code trong Unity

Để tạo một `ScriptableObject` và lưu nó dưới dạng một "prefab" (hoặc nói chính xác hơn là một asset) trong Unity bằng C#, bạn sẽ sử dụng các tính năng của Unity Editor API. Điều quan trọng cần lưu ý là mã này chỉ hoạt động trong Unity Editor và không phải trong build game cuối cùng.

Dưới đây là các bước để tạo và lưu một `ScriptableObject`:

### Bước 1: Định nghĩa Lớp ScriptableObject

Đầu tiên, tạo một lớp `ScriptableObject`. Ví dụ:

```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "NewMyScriptableObject", menuName = "MyScriptableObjects/Create New MyScriptableObject")]
public class MyScriptableObject : ScriptableObject
{
    // Thêm các trường dữ liệu ở đây
    public int intValue;
    public string stringValue;
}
```

### Bước 2: Tạo Script Tạo ScriptableObject

Tạo một script trong Editor để tạo ra `ScriptableObject` mới và lưu nó trong thư mục Assets.

```csharp
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ScriptableObjectCreator
{
    [MenuItem("MyScriptableObjects/Create New MyScriptableObject Asset")]
    public static void CreateMyScriptableObjectAsset()
    {
        MyScriptableObject asset = ScriptableObject.CreateInstance<MyScriptableObject>();
				
				// Tinh chỉnh các thuộc tính tại đây
        asset.intValue = 10; // Giá trị tùy chỉnh cho intValue
        asset.stringValue = "Hello World"; // Giá trị tùy chỉnh cho stringValue

        AssetDatabase.CreateAsset(asset, "Assets/NewMyScriptableObject.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
#endif
```

Trong đoạn mã này:

- `ScriptableObject.CreateInstance<MyScriptableObject>()` tạo ra một instance mới của `MyScriptableObject`.
- `AssetDatabase.CreateAsset()` lưu `ScriptableObject` vào thư mục Assets.
- `AssetDatabase.SaveAssets()` lưu các thay đổi.
- `EditorUtility.FocusProjectWindow()` và `Selection.activeObject` làm cho asset mới được tạo hiển thị trong Unity Editor.

### Bước 3: Sử dụng Script Tạo Asset

- Chạy lệnh "MyScriptableObjects/Create New MyScriptableObject Asset" từ menu của Unity Editor để tạo ra asset mới.
- Asset mới sẽ xuất hiện trong thư mục Assets và bạn có thể chỉnh sửa các thuộc tính của nó từ Inspector trong Unity.

Nhớ rằng mã này chỉ hoạt động trong Unity Editor và không phải là phần của logic game trong build cuối cùng. Điều này rất hữu ích cho việc tự động hóa quy trình làm việc trong quá trình phát triển.

# Unity

## MultiThreading (Đa luồng)

### Khái niệm và ví dụ

Ứng dụng multi-threading trong Unity có thể giúp cải thiện hiệu suất của ứng dụng hoặc game bằng cách phân chia các nhiệm vụ xử lý nặng sang các thread khác nhau, giảm bớt gánh nặng cho main thread. Điều này đặc biệt quan trọng trong các tình huống yêu cầu xử lý nhiều dữ liệu hoặc tính toán phức tạp, như:

1. **Xử lý Hình Ảnh hoặc Dữ Liệu:**
Đối với các tác vụ xử lý hình ảnh nặng nề hoặc phân tích dữ liệu lớn, việc sử dụng multi-threading có thể giúp tăng tốc độ xử lý mà không làm chậm trải nghiệm chính của game.
2. **AI và Tính Toán Đường Đi:**
Trong trường hợp tính toán đường đi cho AI hoặc xử lý logic AI phức tạp, việc chạy các tính toán này trên các thread riêng biệt sẽ giúp tránh việc làm gián đoạn frame rate.
3. **Tải Tài Nguyên Động:**
Việc tải tài nguyên như hình ảnh, âm thanh hoặc dữ liệu từ đĩa cứng có thể được thực hiện trên một thread riêng, giúp tránh hiện tượng "hiccup" trong khi chờ đợi tải.
4. **Tính Toán Vật Lý:**
Nếu bạn đang xử lý một lượng lớn các đối tượng vật lý và các tính toán liên quan, việc chia nhỏ chúng vào các thread riêng có thể giúp cải thiện hiệu suất.

### Ví dụ Cụ thể:

Đây là một ví dụ đơn giản về cách tạo một multi-threading setup trong Unity:

```csharp
using System.Threading;
using UnityEngine;

public class MultiThreadingExample : MonoBehaviour
{
    private void Start()
    {
        // Khởi tạo và bắt đầu nhiều threads
        for (int i = 0; i < 3; i++)
        {
            int threadNumber = i; // Tránh capture của biến vòng lặp trong lambda
            Thread thread = new Thread(() => DoWork(threadNumber));
            thread.Start();
        }
    }

    void DoWork(int threadNumber)
    {
        // Một công việc giả lập nào đó
        for (int i = 0; i < 10; i++)
        {
            Debug.Log($"Thread {threadNumber}: Đang thực hiện công việc {i}");
            Thread.Sleep(500); // Giả lập công việc mất thời gian
        }
    }
}

```

Trong ví dụ trên, chúng ta tạo ba threads khác nhau, mỗi thread thực hiện công việc `DoWork` với một tham số định danh riêng.

### Lưu Ý:

- Khi sử dụng multi-threading, bạn cần đảm bảo rằng không có xung đột hoặc truy cập không an toàn tới các tài nguyên được chia sẻ.
- Các tác vụ liên quan đến Unity API vẫn phải được thực hiện trên main thread.
- Quản lý bộ nhớ

và trạng thái của các thread là rất quan trọng để tránh rò rỉ bộ nhớ hoặc các tình huống "deadlock".

Đối với các ứng dụng Unity, việc quản lý và sử dụng hiệu quả multi-threading đòi hỏi một sự hiểu biết sâu sắc về cách các thread tương tác với nhau và với main thread. Một số kỹ thuật và mẫu thiết kế thông thường bao gồm:

1. **Job System và ECS trong Unity:**
Unity cung cấp hệ thống Job và ECS (Entity Component System) cho phép xử lý song song và hiệu quả cao. Hệ thống Job cho phép bạn định nghĩa công việc có thể chạy song song và được Unity quản lý, giúp giảm thiểu rủi ro khi làm việc với multi-threading.
2. **Locking và Synchronization:**
Khi làm việc với dữ liệu chia sẻ giữa các threads, việc sử dụng các cơ chế khóa (locking) và đồng bộ (synchronization) là cần thiết để tránh tình trạng race condition và đảm bảo tính nhất quán của dữ liệu.
3. **Concurrent Collections:**
Sử dụng các bộ sưu tập dành cho concurrency (như `ConcurrentQueue`, `ConcurrentBag` trong .NET) có thể giúp quản lý dữ liệu chia sẻ một cách an toàn giữa các threads.
4. **Async/Await trong Unity:**
Sử dụng async/await trong C# cũng là một cách hiệu quả để xử lý các tác vụ bất đồng bộ, như tải tài nguyên hoặc gọi API mà không cần tạo threads thủ công.

Nhớ rằng mặc dù multi-threading có thể cải thiện đáng kể hiệu suất, nhưng nó cũng tăng độ phức tạp và tiềm ẩn rủi ro trong việc quản lý và debug. Do đó, việc áp dụng nó cần được cân nhắc kỹ lưỡng dựa trên yêu cầu cụ thể của dự án.

### Vấn đề chặn luồng (blocking thread) và giải pháp

Chặn luồng (Blocking a thread) xảy ra khi một luồng thực thi bị tạm dừng hoặc chờ đợi một điều kiện nào đó hoàn thành trước khi nó có thể tiếp tục. Trong thời gian này, luồng không thể thực hiện bất kỳ công việc nào khác và tài nguyên CPU dành cho luồng đó không được sử dụng hiệu quả.

### Ví dụ về Chặn Luồng:

1. **Đọc Tập Tin Đồng Bộ (Synchronous File Read)**:
    - Giả sử bạn có một ứng dụng cần đọc dữ liệu từ một tập tin lớn. Nếu bạn thực hiện điều này trên luồng chính một cách đồng bộ, luồng chính sẽ bị chặn cho đến khi toàn bộ dữ liệu được đọc xong.
    - Trong thời gian đó, ứng dụng sẽ không phản hồi đối với bất kỳ tương tác nào từ người dùng. Đó là lý do tại sao lập trình không đồng bộ được khuyến khích để xử lý những tác vụ có thể chặn luồng như truy cập mạng hoặc đọc/ghi file lớn.
    
    ```csharp
    public void ReadFile()
    {
        string data = File.ReadAllText("path/to/largefile.txt");
        // Luồng chính bị chặn cho đến khi dữ liệu được đọc hoàn tất
    }
    
    ```
    

### Giải pháp chặn luồng:

Thay vì sử dụng **`File.ReadAllText()`** một cách đồng bộ, chúng ta có thể sử dụng **`File.ReadAllTextAsync()`** để đọc tập tin mà không chặn luồng chính:

Trong ví dụ này, **`ReadFileCoroutine`** sử dụng **`WaitUntil`** để chờ cho đến khi **`Task`** hoàn thành. Điều này giúp đồng bộ hóa tác vụ không đồng bộ với game loop của Unity và cung cấp cách xử lý ngoại lệ tốt hơn

```csharp

using System.Threading.Tasks;
using UnityEngine;
using System.IO;

public class ExampleScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ReadFileCoroutine("path/to/largefile.txt"));
    }

    IEnumerator ReadFileCoroutine(string path)
    {
        Task<string> readFileTask = ReadFileAsync(path);
        yield return new WaitUntil(() => readFileTask.IsCompleted);

        if (readFileTask.Exception != null)
        {
            // Xử lý ngoại lệ
            Debug.LogError(readFileTask.Exception);
        }
        else
        {
            Debug.Log(readFileTask.Result);
        }
    }

    public async Task<string> ReadFileAsync(string path)
    {
        string data = await File.ReadAllTextAsync(path);
        return data;
    }
}
```

### Có cần hủy thread sau khi chạy task trên luồng đó

Khi bạn sử dụng các `Task` trong .NET Framework hoặc .NET Core, bao gồm cả trong Unity, bạn không cần phải lo lắng về việc hủy hoặc quản lý luồng mà `Task` đó chạy trên. Hệ thống quản lý `Task` và luồng của .NET tự động xử lý các khía cạnh này cho bạn. Đây là một số điểm quan trọng:

1. **Quản Lý Tự Động của Luồng**:
    - Khi một `Task` hoàn thành, luồng mà nó chạy trên (nếu nó sử dụng một luồng) sẽ được trả lại cho ThreadPool của .NET để tái sử dụng.
    - Bạn không cần phải hủy hoặc loại bỏ luồng này một cách thủ công.
2. **Tái Sử Dụng Luồng**:
    - .NET sử dụng ThreadPool để tối ưu hóa và quản lý các luồng. Khi một `Task` được hoàn thành, luồng được trả lại vào pool và có thể được tái sử dụng cho các `Task` khác.
    - Điều này giúp giảm chi phí tạo và hủy luồng liên tục.
3. **Giải Phóng Tài Nguyên**:
    - Khi `Task` hoàn thành, tất cả tài nguyên và tham chiếu liên quan đến nó sẽ tự động được thu hồi bởi bộ thu gom rác (Garbage Collector) của .NET.
    - Điều này bao gồm việc giải phóng bất kỳ luồng nào mà `Task` đã sử dụng.
4. **Xử Lý Ngoại Lệ**:
    - Nếu một `Task` kết thúc với một ngoại lệ, ngoại lệ đó sẽ được giữ trong thuộc tính `Exception` của `Task`. Bạn có thể kiểm tra và xử lý nó sau khi `Task` hoàn thành.

Trong hầu hết các trường hợp, bạn chỉ cần tập trung vào logic của `Task` và việc xử lý kết quả hoặc ngoại lệ sau khi `Task` hoàn thành. Hệ thống `Task` và ThreadPool sẽ lo lắng về các vấn đề liên quan đến quản lý luồng.

## Dotween

DotWeen (đôi khi còn được gọi là DOTween) là một thư viện hoạt hình dành cho Unity, nổi tiếng với việc cung cấp các API đơn giản và hiệu quả để tạo ra các hiệu ứng hoạt hình mượt mà. 

- Dưới đây là một số API phổ biến trong DotWeen cùng với ví dụ minh họa bằng C#
    1. **DOFade**: Dùng để làm mờ (fade) đối tượng.
    
    ```csharp
    // Làm mờ một SpriteRenderer từ giá trị alpha hiện tại xuống 0 trong 1 giây
    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
    spriteRenderer.DOFade(0, 1f);
    ```
    
    1. **DOMove**: Di chuyển đối tượng đến một vị trí mới.
    
    ```csharp
    // Di chuyển đối tượng đến vị trí (3, 4, 5) trong 2 giây
    transform.DOMove(new Vector3(3, 4, 5), 2f);
    ```
    
    3. **DORotate**: Quay đối tượng.
    
    ```csharp
    // Quay đối tượng 90 độ quanh trục Y trong 3 giây
    transform.DORotate(new Vector3(0, 90, 0), 3f);
    ```
    
    1. **DOScale**: Thay đổi kích thước của đối tượng.
    
    ```csharp
    // Phóng to kích thước đối tượng lên gấp đôi trong 1 giây
    transform.DOScale(2, 1f);
    ```
    
    1. **DOColor**: Thay đổi màu sắc.
    
    ```csharp
    // Thay đổi màu của SpriteRenderer sang màu xanh lá trong 1 giây
    spriteRenderer.DOColor(Color.green, 1f);
    ```
    
    1. **DOPunchPosition**: Tạo hiệu ứng giật lắc vị trí.
    
    ```csharp
    // Tạo hiệu ứng giật lắc vị trí của đối tượng
    transform.DOPunchPosition(new Vector3(0.5f, 0, 0), 1f, 10, 0.5f);
    ```
    
    1. **DOShakePosition**: Tạo hiệu ứng rung động vị trí.
    
    ```csharp
    // Tạo hiệu ứng rung động vị trí
    transform.DOShakePosition(1f, 1f);
    ```
    
    1. **DOPath**: Di chuyển đối tượng theo một đường dẫn.
    
    ```csharp
    // Di chuyển theo đường cong
    Vector3[] path = new Vector3[] { new Vector3(0, 0, 0), new Vector3(2, 2, 0), new Vector3(4, 0, 0) };
    transform.DOPath(path, 2f, PathType.CatmullRom);
    ```
    
    1. **DOTween.Sequence()**: Kết hợp nhiều hoạt hình.
    
    ```csharp
    // Tạo một chuỗi hoạt hình
    Sequence mySequence = DOTween.Sequence();
    mySequence.Append(transform.DOMoveX(5, 1f));
    mySequence.Join(spriteRenderer.DOColor(Color.red, 1f));
    mySequence.Append(transform.DOMoveY(5, 1f));
    mySequence.Append(transform.DOScale(0.5f, 1f));
    ```
    
    1. **DOBlendableColor**: Thay đổi màu sắc có thể kết hợp.
    
    ```csharp
    // Thay đổi màu sắc của SpriteRenderer một cách có thể kết hợp
    spriteRenderer.DOBlendableColor(Color.blue, 1f);
    ```
    
    1. **DOFillAmount**: Thay đổi tỉ lệ điền đầy của UI Image.
    
    ```csharp
    // Thay đổi tỉ lệ điền đầy của UI Image
    Image image = GetComponent<Image>();
    image.DOFillAmount(0.5f, 2f);
    ```
    
    1. **DOLocalMove**: Di chuyển đối tượng tới một vị trí mới dựa trên hệ tọa độ cục bộ.
    
    ```csharp
    // Di chuyển đối tượng đến vị trí cục bộ mới
    transform.DOLocalMove(new Vector3(3, 3, 0), 2f);
    ```
    
    1. **DOLocalRotate**: Quay đối tượng dựa trên hệ tọa độ cục bộ.
    
    ```csharp
    // Quay đối tượng cục bộ 90 độ quanh trục Z
    transform.DOLocalRotate(new Vector3(0, 0, 90), 2f, RotateMode.Fast);
    ```
    
    1. **DOJump**: Tạo hiệu ứng nhảy cho đối tượng.
    
    ```csharp
    // Nhảy đến vị trí mới với độ cao 2, 3 lần nhảy trong 2 giây
    transform.DOJump(new Vector3(5, 0, 0), 2, 3, 2f);
    ```
    
    1. **DOText**: Thay đổi nội dung của Text.
    
    ```csharp
    // Thay đổi nội dung của UI Text thành "Hello World!" trong 2 giây
    Text uiText = GetComponent<Text>();
    uiText.DOText("Hello World!", 2f);
    ```
    
    1. **DOTimeScale**: Thay đổi tốc độ hoạt hình (time scale).
    
    ```csharp
    // Chậm lại hoạt hình (time scale) xuống 0.5 trong 1 giây
    DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.5f, 1f);
    ```
    
    1. **DOLocalPath**: Di chuyển đối tượng theo một đường dẫn cục bộ.
    
    ```csharp
    // Di chuyển theo đường dẫn cục bộ
    Vector3[] localPath = new Vector3[] { new Vector3(0, 0, 0), new Vector3
    ```
    
    1. **DOShakeScale**: Tạo hiệu ứng rung động cho kích thước đối tượng.
    
    ```csharp
    // Rung động kích thước đối tượng
    transform.DOShakeScale(1f, new Vector3(0.5f, 0.5f, 0), 10, 90);
    ```
    
    1. **DORewind**: Quay trở lại trạng thái ban đầu của hoạt hình.
    
    ```csharp
    // Sau khi thực hiện một số hoạt hình, gọi DORewind để quay trở lại trạng thái ban đầu
    mySequence.DORewind();
    ```
    
    1. **DOPause**: Tạm dừng hoạt hình.
    
    ```csharp
    // Tạm dừng hoạt hình
    mySequence.DOPause();
    ```
    
    1. **DOPlay**: Bắt đầu hoặc tiếp tục hoạt hình.
    
    ```csharp
    // Bắt đầu hoặc tiếp tục hoạt hình
    mySequence.DOPlay();
    ```
    
    1. **DOFlip**: Đảo ngược hướng của hoạt hình.
    
    ```csharp
    // Đảo ngược hướng hoạt hình
    mySequence.DOFlip();
    ```
    
    1. **DOKill**: Ngừng hoạt hình và loại bỏ nó khỏi bộ nhớ.
    
    ```csharp
    // Ngừng hoạt hình và loại bỏ nó
    mySequence.DOKill();
    ```
    
    1. **DOComplete**: Hoàn thành ngay lập tức hoạt hình hiện tại.
    
    ```csharp
    // Hoàn thành ngay lập tức hoạt hình
    mySequence.DOComplete();
    ```
    
    1. **DOPlayBackwards**: Chạy hoạt hình theo hướng ngược lại.
    
    ```csharp
    // Chạy hoạt hình theo hướng ngược lại
    mySequence.DOPlayBackwards();
    ```
    
    1. **DOPlayForward**: Chạy hoạt hình theo hướng tiến lên.
    
    ```csharp
    // Chạy hoạt hình theo hướng tiến lên
    mySequence.DOPlayForward();
    ```
    
    1. **DORestart**: Khởi động lại hoạt hình từ đầu.
    
    ```csharp
    // Khởi động lại hoạt hình từ đầu
    mySequence.DORestart();
    ```
    
    1. **DOBlendableMoveBy**: Di chuyển đối tượng một cách linh hoạt và có thể kết hợp với các hoạt hình khác.
    
    ```csharp
    // Di chuyển đối tượng một cách linh hoạt
    transform.DOBlendableMoveBy(new Vector3(3, 0, 0), 2f);
    ```
    
    1. **DOBlendableRotateBy**: Quay đối tượng một cách linh hoạt và có thể kết hợp với các hoạt hình khác.
    
    ```csharp
    // Quay đối tượng một cách linh hoạt
    transform.DOBlendableRotateBy
    ```
    
    Trong các ví dụ trên, mỗi API tương ứng với một hiệu ứng hoạt hình cụ thể và có thể được tùy chỉnh linh hoạt. DOTween cho phép bạn tạo ra các hiệu ứng hoạt hình mượt mà và ấn tượng mà không cần viết quá nhiều mã lập trình phức tạp.
    

## Async

Async/await trong C# là một cách tiếp cận quan trọng để thực hiện các tác vụ không đồng bộ. Đây không phải là các phương thức cụ thể, mà là từ khóa được sử dụng để đánh dấu các phương thức không đồng bộ và tạm dừng thực thi của chúng mà không chặn luồng thực thi chính.

Async/await là một phần mạnh mẽ của C# và .NET, cho phép bạn viết code không đồng bộ một cách dễ đọc và hiệu quả, giảm thiểu vấn đề về việc chặn luồng và làm cho ứng dụng của bạn trở nên mượt mà hơn. 

Dưới đây là một số khái niệm và từ khóa chính liên quan đến async/await trong C#:

### 1. `async` Modifier

- **Định nghĩa:** Từ khóa `async` được thêm vào phương thức để biểu thị rằng phương thức đó là không đồng bộ.
- **Ví dụ:**
    
    ```csharp
    public async Task MyAsyncMethod()
    {
        // ...
    }
    
    ```
    

### 2. `await` Keyword

- **Định nghĩa:** Từ khóa `await` được sử dụng để tạm dừng thực thi của phương thức không đồng bộ cho đến khi tác vụ không đồng bộ (Task) hoàn tất.
- **Ví dụ:**
    
    ```csharp
    await SomeAsyncOperation();
    
    ```
    

### 3. `Task`

- **Định nghĩa:** Lớp `Task` đại diện cho một tác vụ không đồng bộ.
- **Ví dụ:**
    
    ```csharp
    public async Task DoSomethingAsync()
    {
        // ...
    }
    
    ```
    

### 4. `Task<TResult>`

- **Định nghĩa:** `Task<TResult>` là một biến thể của `Task` có thể trả về một giá trị.
- **Ví dụ:**
    
    ```csharp
    public async Task<int> CalculateValueAsync()
    {
        // ...
    }
    
    ```
    

### 5. `Task.Run()`

- **Định nghĩa:** `Task.Run()` được sử dụng để chạy một tác vụ không đồng bộ trên luồng thread pool.
- **Ví dụ:**
    
    ```csharp
    var result = await Task.Run(() => CalculateSomething());
    
    ```
    

### 6. `Task.WhenAll()`

- **Định nghĩa:** Chờ cho đến khi tất cả các `Task` đã cho hoàn thành.
- **Ví dụ:**
    
    ```csharp
    await Task.WhenAll(task1, task2, task3);
    
    ```
    

### 7. `Task.WhenAny()`

- **Định nghĩa:** Chờ cho đến khi bất kỳ `Task` nào trong một tập hợp các `Task` hoàn thành.
- **Ví dụ:**
    
    ```csharp
    await Task.WhenAny(task1, task2);
    
    ```
    

### 8. `Task.Delay()`

- **Định nghĩa:** Tạo một tác vụ hoàn thành sau một khoảng thời gian xác định.
- **Ví dụ:**
    
    ```csharp
    await Task.Delay(1000); // Delay 1 giây
    
    ```
    

### 9. `Task.Yield()`

- **Định nghĩa:** Tạm dừng thực thi của async method và trả về luồng điều khiển ngay lập tức.
- **Ví dụ:**
    
    ```csharp
    await Task.Yield();
    
    ```
    

### 10. `ConfigureAwait(false)`

- **Định nghĩa:** Chỉ định liệu async method có quay trở lại context gọi ban đầu sau khi hoàn thành hay không.
- **Ví dụ:**
    
    ```csharp
    await SomeAsyncMethod().ConfigureAwait(false);
    
    ```
    

### 11. Asynchronous Stream (`IAsyncEnumerable<T>`)

- **Định nghĩa:** Cho phép bạn viết và sử dụng các stream mà các phần tử được tạo và tiêu thụ một cách không đồng bộ.
- **Ví dụ:**
    
    ```csharp
    await foreach (var item in GetAsyncEnumerable())
    {
        // Xử lý mỗi item khi nó được sản xuất
    }
    
    ```
    

### 12. `ValueTask` và `ValueTask<TResult>`

- **Định nghĩa:** Các biến thể của `Task` và `Task<TResult>` cho phép hiệu suất cao hơn trong một số tình huống nhất định, đặc biệt là khi kết quả có thể trở thành sẵn có mà không cần chờ đợi thực sự.
- **Ví dụ:**
    
    ```csharp
    public async ValueTask<int> FastCalculationAsync()
    {
        // ...
    }
    
    ```
    

### 13. Cancellation Token và `Task`

- **Định nghĩa:** Cho phép hủy bỏ một tác vụ không đồng bộ đang chạy.
- **Ví dụ:**
    
    ```csharp
    var cancellationTokenSource = new CancellationTokenSource();
    var task = SomeLongRunningTaskAsync(cancellationTokenSource.Token);
    // Hủy task nếu cần
    cancellationTokenSource.Cancel();
    
    ```
    

### 14. Async Lambda

- **Định nghĩa:** Lambda không đồng bộ, có thể sử dụng `await` trong nó.
- **Ví dụ:**
    
    ```csharp
    Func<Task> asyncLambda = async () => { await Task.Delay(1000); };
    
    ```
    

### 15. Async Main Method

- **Định nghĩa:** Cho phép phương thức `Main` của ứng dụng console chạy không đồng bộ.
- **Ví dụ:**
    
    ```csharp
    public static async Task Main(string[] args)
    {
        await SomeAsyncOperation();
    }
    
    ```
    

### 16. Async Constructors

- **Chú Ý:** Trong C#, không thể tạo async constructors trực tiếp. Tuy nhiên, bạn có thể sử dụng một phương thức factory async để khởi tạo một đối tượng một cách không đồng bộ.
- **Ví dụ:**
    
    ```csharp
    public class MyClass
    {
        private MyClass() { }
        public static async Task<MyClass> CreateAsync()
        {
            var myClass = new MyClass();
            await myClass.InitializeAsync();
            return myClass;
        }
        private async Task InitializeAsync() { /* Async initialization logic */ }
    }
    
    ```
    

### 17. Exception Handling

- **Định nghĩa:** Xử lý ngoại lệ trong các tác vụ không đồng bộ.
- **Ví dụ:**
    
    ```csharp
    try
    {
        await SomeAsyncMethod();
    }
    catch (Exception ex)
    {
        // Xử lý ngoại lệ
    }
    
    ```
    

Async/await mang đến cách tiếp cận mạnh mẽ và linh hoạt để xử lý các hoạt động không đồng bộ trong ứng dụng C#, giúp việc lập trình trở nên dễ dàng và rõ ràng hơn, đặc biệt trong môi trường đa luồng và các tác vụ chờ đợi dài hạn.

## Thao tác tranform GameObject

- Lấy phần tử child-GameObject đầu tiên trong parent-GameObject
    
    ```csharp
    using UnityEngine;
    
    public class ChildIterator : MonoBehaviour
    {
        public GameObject parentObject;
    
        void Start()
        {
            if (parentObject != null)
            {          
                GameObject child = parentObject.transform.GetChild(0).gameObject;                       
            }
        }
    }
    ```
    
- Duyệt qua tất cả các GameObject con (child GameObjects) của một GameObject cha
    
    ```csharp
    using UnityEngine;
    
    public class ExampleScript : MonoBehaviour
    {
        void Start()
        {
            Transform parentTransform = transform; // 'transform' là Transform của GameObject mà script này đính kèm
    
            foreach (Transform child in parentTransform)
            {
                // Thực hiện hành động với mỗi child transform
                Debug.Log("Child GameObject: " + child.gameObject.name);
            }
        }
    }
    ```
    
- Đếm số lượng phần tử child-GameObject
    
    ```csharp
    using UnityEngine;
    
    public class CountChildren : MonoBehaviour
    {
        public GameObject parentObject;
    
        void Start()
        {
            // Kiểm tra xem parentObject có được gán hay không
            if (parentObject != null)
            {
                // Đếm số lượng GameObject con
                int numberOfChildren = parentObject.transform.childCount;
                Debug.Log($"Số lượng GameObject con: {numberOfChildren}");
            }
        }
    }
    ```
    
- Lấy Component cha khi biết child-GameObject
    
    Trong Unity, `Component.GetComponentInParent<T>()` là một phương thức được sử dụng để tìm một thành phần cụ thể của kiểu `T` trên GameObject hiện tại hoặc trên một trong các GameObject cha của nó trong hệ thống phân cấp (hierarchy).
    
    ### Ví dụ:
    
    Giả sử bạn có một hệ thống phân cấp GameObject như sau:
    
    - **ParentObject** (có một thành phần là `Rigidbody`)
        - **ChildObject** (đây là GameObject mà script của bạn đang được gắn vào)
    
    Nếu bạn muốn tìm `Rigidbody` từ script trên `ChildObject`, bạn sẽ sử dụng `GetComponentInParent` như sau:
    
    ```csharp
    using UnityEngine;
    
    public class ExampleScript : MonoBehaviour
    {
        void Start()
        {
            // Tìm Rigidbody trên GameObject cha
            Rigidbody rb = GetComponentInParent<Rigidbody>();
    
            if (rb != null)
            {
                // Làm gì đó với Rigidbody
            }
        }
    }
    
    ```
    
    Trong ví dụ này, `GetComponentInParent<Rigidbody>()` sẽ tìm kiếm lên phía GameObject cha (`ParentObject`) để tìm thành phần `Rigidbody`. Nếu tìm thấy, biến `rb` sẽ được gán với tham chiếu đến `Rigidbody` đó, và bạn có thể thực hiện các hành động tiếp theo với nó.
    
- Di chuyển GameObject lên hàng đầu trong Hierachy
    
    ```csharp
    using UnityEngine;
    
    public class MoveToTopOfHierarchy : MonoBehaviour
    {
        public GameObject objectToMove;
    
        void Start()
        {
            // Kiểm tra xem objectToMove có được gán hay không
            if (objectToMove != null)
            {
                // Di chuyển objectToMove lên đầu trong Hierarchy
                objectToMove.transform.SetAsFirstSibling();
            }
        }
    }
    ```
    
- Di chuyển GameObject xuống cuối hàng trong Hierachy
    
    ```csharp
    using UnityEngine;
    
    public class MoveToBottom : MonoBehaviour
    {
        void Start()
        {
            // Đặt GameObject này xuống cuối của Hierarchy
            transform.SetAsLastSibling();
        }
    }
    ```
    

## Coroutine

****`WaitUntil`** 

- Là một class trong Unity dùng trong coroutines để tạm dừng việc thực thi của coroutine cho đến khi một điều kiện nhất định được thỏa mãn.
- Ví dụ:
    
    ```markdown
    [https://www.notion.so/Cheat-Sheet-12dc564325ed4472b688f4729dc13ff5?pvs=4#22684107659a41e4bce0dce843eb4fbb](https://www.notion.so/Cheat-Sheet-12dc564325ed4472b688f4729dc13ff5?pvs=21)
    ```
    

****`WaitWhile`****

- Tạm dừng coroutine cho đến khi một điều kiện nhất định trở nên sai.
    
    ```csharp
    yield return new WaitWhile(() => condition == true);
    ```
    

****`StartCoroutine`****

- Tạm dừng coroutine cho đến khi coroutine khác hoàn thành.
    
    ```csharp
    yield return StartCoroutine(AnotherCoroutine());
    ```
    

****`WaitForSeconds`****

- Tạm dừng coroutine trong một khoảng thời gian cố định (tính bằng giây).
    
    ```csharp
    yield return new WaitForSeconds(2.5f);
    ```
    

****`WaitForFixedUpdate`****

- Tạm dừng coroutine cho đến lần cập nhật vật lý (physics update) tiếp theo.
    
    ```csharp
    yield return new WaitForFixedUpdate();
    ```
    

****`WaitForEndOfFrame`****

- Tạm dừng coroutine cho đến khi kết thúc khung hình hiện tại.
    
    ```csharp
    yield return new WaitForEndOfFrame();
    ```
    

****`WWW`****

- Tạm dừng coroutine cho đến khi hoàn thành yêu cầu mạng. (Lưu ý: **`WWW`** đã lỗi thời và nên thay thế bằng **`UnityWebRequest`**).

```csharp
yield return new WWW(url);
```

****`AsyncOperation`****

- Dùng cho các hoạt động không đồng bộ như tải cảnh (scene) hoặc tài nguyên.
    
    ```csharp
    yield return new AsyncOperation;
    ```
    

## FireBase

## Vector

• Dùng để biểu diễn hướng và độ lớn trong không gian 2D hoặc 3D. Unity cung cấp một số loại vector khác nhau, phổ biến nhất là Vector2, Vector3, và Vector4.

**Các thuộc tính tĩnh của Vector3**

```csharp
Vector3.zero        Vectơ với tất cả các thành phần là 0 ((0, 0, 0)).
Vector3.one         Vectơ với tất cả các thành phần là 1 ((1, 1, 1)).
Vector3.forward     Vectơ chỉ hướng phía trước ((0, 0, 1)).
Vector3.back        Vectơ chỉ hướng phía sau ((0, 0, -1)).
Vector3.up          Vectơ chỉ hướng lên trên ((0, 1, 0)).
Vector3.down        Vectơ chỉ hướng xuống dưới ((0, -1, 0)).
Vector3.left        Vectơ chỉ hướng sang trái ((-1, 0, 0)).
Vector3.right       Vectơ chỉ hướng sang phải ((1, 0, 0)).

//Đại diện cho giá trị vectơ vô cực trong tính toán.
Vector3.positiveInfinity    Vectơ với tất cả các thành phần là float.PositiveInfinity.
//Đại diện cho giá trị vectơ âm vô cực trong tính toán.
Vector3.negativeInfinity    Vectơ với tất cả các thành phần là float.NegativeInfinity.
```

**Các thuộc tính của Vector3 (Properties)**

```csharp
//magnitude: Thuộc tính magnitude trả về độ dài của vectơ 
//(tính bằng cách sử dụng định lý Pythagoras).
Vector3 position = new Vector3(3, 4, 0);
float length = position.magnitude; // length = 5

//normalized: Thuộc tính normalized trả về phiên bản chuẩn hóa của vectơ, 
//có cùng hướng nhưng độ dài bằng 1.
Vector3 position = new Vector3(3, 4, 0);
Vector3 normalizedPosition = position.normalized;
```

**Các public method của vector3**

```csharp
//Equals: Phương thức Equals được sử dụng để so sánh một Vector3 với một đối tượng 
//khác hoặc một Vector3 khác để xác định xem chúng có bằng nhau hay không.
Vector3 vectorA = new Vector3(1, 2, 3);
Vector3 vectorB = new Vector3(1, 2, 3);
bool isEqual = vectorA.Equals(vectorB); // isEqual sẽ là true

//Set: Phương thức Set cho phép bạn thiết lập lại các giá trị x, y, và z của 
//một Vector3 mà không cần tạo một instance mới.
Vector3 vector = new Vector3(1, 2, 3);
vector.Set(4, 5, 6); // Bây giờ vector sẽ là (4, 5, 6)

//ToString: Phương thức ToString chuyển đổi Vector3 thành chuỗi (string), 
//hữu ích cho việc in thông tin ra console hoặc hiển thị trên giao diện người dùng.
Vector3 vector = new Vector3(1.2345f, 2.3456f, 3.4567f);
//// vectorAsString sẽ là "1.23, 2.35, 3.46"
string vectorAsString = vector.ToString("F2"); 
```

## Input

### Input mouse

### Input system

## Di chuyển đối tượng

• Có nhiều cách khác nhau để di chuyển một đối tượng, mỗi cách có ưu và nhược điểm riêng tùy thuộc vào yêu cầu cụ thể của trò chơi hoặc ứng dụng của bạn. Dưới đây là một số phương pháp chính:

### Sử dụng Transform

- Transform.position: Đặt trực tiếp vị trí của đối tượng. Phù hợp cho di chuyển tức thời đến một vị trí cụ thể
    
    ```csharp
    using UnityEngine;
    
    public class SimplePositionMovement : MonoBehaviour
    {
        public float speed = 5.0f; // Tốc độ di chuyển
    
        void Update()
        {
            // Lấy đầu vào từ bàn phím
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
    
            // Tạo vectơ di chuyển dựa trên đầu vào
            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * speed * Time.deltaTime;
    
            // Cập nhật vị trí mới cho đối tượng
            // Thay đổi trực tiếp thuộc tính 'position' của Transform
            transform.position += movement;
        }
    }
    ```
    
- Transform.Translate: Di chuyển đối tượng tương đối so với vị trí hiện tại. Thích hợp cho di chuyển liên tục hoặc theo đầu vào người chơi.
    
    ```csharp
    using UnityEngine;
    
    public class SimpleMovement : MonoBehaviour
    {
        public float speed = 5.0f; // Tốc độ di chuyển
    
        void Update()
        {
            // Lấy đầu vào từ bàn phím
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
    
            // Tạo vectơ di chuyển dựa trên đầu vào
            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
    
            // Di chuyển đối tượng
            // Translate sẽ di chuyển đối tượng theo hướng của vectơ 'movement'
            // Time.deltaTime đảm bảo di chuyển mượt mà và độc lập với tốc độ khung hình
            transform.Translate(movement * speed * Time.deltaTime);
        }
    }
    ```
    

### **Sử dụng Rigidbody (Vật lý)**

- Rigidbody.MovePosition: Di chuyển đối tượng theo vật lý, phù hợp cho việc di chuyển mà cần xét tới va chạm và vật lý.
    
    ```csharp
    using UnityEngine;
    
    public class RigidbodyMovement : MonoBehaviour
    {
        public float speed = 5.0f;
        private Rigidbody rb;
    
        void Start()
        {
            // Lấy component Rigidbody
            rb = GetComponent<Rigidbody>();
        }
    
        void FixedUpdate()
        {
            // Lấy đầu vào từ bàn phím
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
    
            // Tạo vectơ di chuyển
            Vector3 movement = new Vector3(horizontal, 0, vertical);
    
            // Di chuyển Rigidbody
            // Sử dụng MovePosition để di chuyển một cách mượt mà và tuân thủ vật lý
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }
    ```
    
- Rigidbody.AddForce: Áp dụng lực để di chuyển đối tượng. Tốt cho di chuyển tự nhiên và thực tế hơn.
    
    ```csharp
    using UnityEngine;
    
    public class ForceMovement : MonoBehaviour
    {
        public float forceAmount = 10.0f;
        private Rigidbody rb;
    
        void Start()
        {
            // Lấy component Rigidbody của đối tượng
            rb = GetComponent<Rigidbody>();
        }
    
        void FixedUpdate()
        {
            // Lấy đầu vào từ bàn phím
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
    
            // Tạo vectơ lực dựa trên đầu vào
            Vector3 force = new Vector3(horizontal, 0, vertical) * forceAmount;
    
            // Áp dụng lực vào Rigidbody
            // Điều này sẽ di chuyển đối tượng dựa trên vật lý
            rb.AddForce(force);
        }
    }
    ```
    

### Sử dụng **CharacterController**

- CharacterController.Move: Điều khiển di chuyển của nhân vật, tự xử lý va chạm và mô phỏng một số vật lý.
- Trong Inspector, nhấp vào "Add Component" và tìm kiếm "Character Controller". Thêm thành phần "Character Controller" vào nhân vật.
- Điều chỉnh các thiết lập của CharacterController trong Inspector để phù hợp với kích thước và hành vi mong muốn của nhân vật (ví dụ: chiều cao, bán kính, v.v.).
- Trong phương thức Update hoặc FixedUpdate, sử dụng CharacterController.Move() để di chuyển nhân vật.
- Lưu ý rằng CharacterController không sử dụng hệ thống vật lý của Unity để va chạm, vì vậy nó cung cấp nhiều kiểm soát hơn nhưng đồng thời bạn cũng cần tự xử lý va chạm và tương tác với môi trường nếu cần.
- Dưới đây là ví dụ về một script cơ bản để di chuyển nhân vật sử dụng CharacterController:
    
    ```csharp
    using UnityEngine;
    public class PlayerMovement : MonoBehaviour
    {
        //tốc độ di chuyển của nhân vật
        public float speed = 5.0f;
        //tham chiếu đến CharacterController
        private CharacterController controller;
    
        void Start()
        {
            controller = GetComponent<CharacterController>();
        }
    
        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(horizontal, 0, vertical);
            //di chuyển nhân vật dựa trên vectơ di chuyển đã tính.
            controller.Move(movement * speed * Time.deltaTime);
        }
    }
    ```
    

### **Sử dụng Lerping và Slerping**

### Sử dụng Dotween

## Xoay đối tượng

## Xử lý va chạm, kích hoạt (Collider, Collision)

## Camera

## Khởi tạo và hủy đối tượng

## Tìm kiếm đối tượng

## Thao tác với component

1. Lấy component trong đối tượng GameObject
    
    ```csharp
    using UnityEngine;
    
    public class ExampleScript : MonoBehaviour
    {
        private Rigidbody rb;
    
        void Start()
        {
            // Lấy component Rigidbody và lưu vào biến 'rb'
            rb = GetComponent<Rigidbody>();
    
            // Kiểm tra nếu Rigidbody không tồn tại trên GameObject này
            if (rb == null)
            {
                Debug.LogError("Rigidbody not found on " + gameObject.name);
            }
        }
    }
    ```
    
2. Lấy tất cả các component trong một đối tượng GameObject
    
    ```csharp
    using UnityEngine;
    
    public class GetAllComponentsExceptBoxCollider2D : MonoBehaviour
    {
        void Start()
        {
            // Lấy tất cả các component
            Component[] components = GetComponents<Component>();
    
            // Duyệt qua từng component
            foreach (var component in components)
            {
                // Kiểm tra nếu component không phải là BoxCollider2D
                if (!(component is BoxCollider2D))
                {
                    // Làm gì đó với component
                    Debug.Log("Component found: " + component.GetType());
                }
            }
        }
    }
    ```
    
3. **`TryGetComponent`** là một phương thức trong Unity cho phép bạn cố gắng lấy một component từ một GameObject. Nếu thành công, nó sẽ trả về **`true`** và cung cấp tham chiếu đến component đó; nếu không, nó trả về **`false`**. Phương thức này hữu ích vì nó giúp tránh ném ra ngoại lệ nếu component không tồn tại và là một cách tối ưu hơn để kiểm tra và lấy component.
    
    ```csharp
    using UnityEngine;
    
    public class ExampleScript : MonoBehaviour
    {
        void Start()
        {
            // Thử lấy component Rigidbody
            if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                // Nếu thành công, làm gì đó với Rigidbody
                rb.useGravity = false;
                Debug.Log("Rigidbody found and gravity turned off.");
            }
            else
            {
                // Nếu không thành công, xử lý trường hợp không có Rigidbody
                Debug.Log("Rigidbody not found on this GameObject.");
            }
        }
    }
    ```
    
4. Hủy một Component trên GameObject
    
    ```csharp
    using UnityEngine;
    
    public class DestroyComponentExample : MonoBehaviour
    {
        void Start()
        {
            // Lấy tham chiếu đến component Rigidbody
            Rigidbody rb = GetComponent<Rigidbody>();
    
            // Kiểm tra nếu Rigidbody tồn tại
            if (rb != null)
            {
                // Hủy component Rigidbody
                Destroy(rb);
            }
        }
    }
    ```
    

## UI Canvas

### Cài đặt canvas

### Thao tác với các UI

## Animation

## Layer và Tag

## Material

## Particle System (Hệ thống hạt)

## Invoke

## Scene

## Sprite

## Sound

## Video player

## Delegate trong Unity

## Event trong Unity

## Các thuộc tính dùng trong Inspector

## Thao tác với Prefab

# C#

## Thao tác list

## Thao tác chuỗi

## Làm tròn số

## LinQ

## Ép kiểu

## Properties

## Enum

## Lamda

## Break, return, continue

# Algothirm

- 2. Nếu bạn muốn truy cập một **Component** trong một **GameObject** cụ thể, hãy đưa **GameObject** đó vào **SerializeField**, sau đó gọi hàm `GetComponent()` với đối số là loại **Component** muốn lấy.
    
    1. Dùng biến `static` ****để lưu trữ giá trị dùng chung giữa tất các **scene** trong game.
    
- 3. Riêng `GameObject` ****đã gán bằng ****`SerializeFiled` ****nó rồi thì không cần phải khởi tạo nó trong **Awake().**
- 4. Dùng `GetComponent()` khi object đó có đính kèm **Component** mình muốn lấy. `FindObjectOfType()` khi trong scence có loại object tương thích, nó sẽ trả về object tìm kiếm đầu tiên.
- 5. Coroutine
    - Khi một **Gameobject** bị **destroy** hoặc **disable** rồi thì tất cả **Coroutine** đính ****theo nó sẽ dừng hoạt động.
    - Chúng ta có thể kết hợp giữa **Coroutine và while()** để di chuyển, xoay, phóng to nhỏ hoặc thay đổi giá trị của một vật thể nào đó theo thời gian.
        
        ```csharp
        IEnumerator ZoomOutAndDestroy(ObjectController destroyObj)
            {
                while (destroyObj != null)
                {
                    if (destroyObj != null)
                    {
                        float deltaScaling = scaleSpeed * Time.deltaTime * 12;
                        Vector2 currentScale = destroyObj.transform.localScale;
                        if (currentScale.y > originalScale.y * 0.2)
                        {
                            destroyObj.transform.localScale = new Vector2(currentScale.x - deltaScaling * originalScale.x
                                                                            , currentScale.y - deltaScaling * originalScale.y);
                        }
                        else
                        {
                            Destroy(destroyObj.gameObject);
                        }
                    }
        						//Run 100 times each 1 second
                    yield return new WaitForSeconds(0.01f);
                }
            }
        ```
        
- 6. Các **object** có trong scence mới gán **Slider** *SerializeFiled* vào được, **Prefabs** không gán được
- Tạo **CanVas** đầu tiên phải chú ý đến mục **Canvas Scaler**
- Làm tròn số thực 2 chữ số sau dấu thập phân
    
    ```jsx
    yourFloat = Mathf.Round(yourFloat * 100f) / 100f;
    ```
    
- Khi khai báo 1 biến `List<type>` thì **phải khởi tạo nó ngay lúc khai báo biến hoặc trong Awake** mới dùng được.
- Sử dụng enum để tạo 1 list danh sách có thể chọn được.
    
    ```csharp
    public enum GemType {Blue, Green, Orange, Pink, Red, Turquoise, Yellow};
    [SerializeField] GemType type;
    ```
    
- Tìm các object có tên chứa từ “*new*” ta dùng `gameObject.name.Contains(”new”)`
- Tìm gameObject trong scene bằng lệnh `GameObject.Find()`
- Muốn tắt hoặc mở một **GameObject** ta dùng **GameObject.SetActive(false/true)**
- Muốn tắt hoặc mở một Component ta dùng `Component.active` **= false/true**
- Lưu ý!!! Lệnh **break** chỉ thoát khỏi 1 vòng lặp.
- Lưu ý!!! đổi tên biến được set `SerializeField` → Object bên ngoài sẽ bị missing → phải gán lại
- Lưu ý!!! Không nên lạm dụng `FindObjectOfType()` **đặt ở** void `Awake()` để lấy những đối tượng chưa được tạo trước khi chạy. Vì nếu lệnh `FindObjectOfType()` ****chạy trước khi khởi tạo đối tượng đó sẽ gây ra lỗi **nullreferenceexception**. Vì thế chúng ta nên lấy tại đúng vị trí cần dùng đối tượng đó.
- Để gộp 2 mảng List<> lại với nhau ta dùng `AddRange()` method
    
    ```csharp
    List<int> first = new List<int>() { 1, 2, 3, 4, 5 };
    List<int> second = new List<int>() { 6, 7, 8, 9 };
    first.AddRange(second)
    ```
    
- Nếu thêm **animation** vào trong **Object** sẽ **không thay đổi được tranform** hoặc các thuộc tính khác → nguyên nhân là do đang bị trạng trạng thái **Idle animatior** kiểm soát.
- Muốn di chuyển đối tượng đến 1 điểm khác, ta dùng `Vector2.MoveTowards()` hoặc dùng `Vector2.Lerp()`
    
    
- Để duplicate (sao chép) một object thì ta dùng `Instantiate()`
- Muốn lấy ngẫu nhiên một số bất kỳ trong dãy số
    
    → Ta dùng **int randomNum = Random.Range(minRange, maxRange);**
    
- **ParticleSystem**
    - Dùng **Gravity Modifier** để tạo hiệu ứng trong lực cho các hạt rơi xuống
    - Nếu các hạt bị che dưới các lớp layer
        
        → Nếu có adding materrial (không để defaut), sửa chỗ **Rendering Mode : Fade**, **Color Mode: Multiply** 
        
        → Xem ở mục **********Renderer**********, ở gần dưới cuối, chỉnh chỗ Order in Layer.
        
    - Nếu muốn tạo các hạt bắn ra 1 loạt
        
        → Đặt tất cả **Rate over Time/Distance** giá trị bằng 0
        
        → Bật **Bursts** lên
        
- **UI Mask** Component → Nghiên cứu thêm ngoài giờ
    - Mask xử lí trên Canvas nên dùng mask phải để vào trong Canvas
    - Mask hoạt động với Image Component, không dùng Mask với Sprite Renderer
    - Dùng để chặn UI component ra ngoài bound của khung hình
    - Tham khảo: [https://learn.unity.com/tutorial/ui-masking#](https://learn.unity.com/tutorial/ui-masking#)
- Dùng **Properties** (thuộc) để **get** & **set** → Rút gắn code đơn giản, gọn gàng hơn.
    
    Ví dụ:
    
    ```csharp
    //The number of videos created 
    [SerializeField] int numVidCreated = 0;
    #region Public Properties
    public int NumVidCreated { get { return numVidCreated; } set { numVidCreated = value; } }
    #endregion
    ```
    
    Hoặc có cách rút gọn hơn bằng Lambda như sau
    
    ```csharp
    public int NumVidCreated { get => numVidCreated; set => numVidCreated = value; }
    ```
    
- Muốn chỉnh order layer cho **UI** (text, button, image,…)
    - Ta chỉnh order layer Canvas đính kèm UI đó.
    - Tuy nhiên ta muốn tùy chỉnh order layer riêng cho từng đối tượng trong UI Canvas. Ta có thể thử Sprite Render.
- **Video Player**
    1. Nếu muốn tắt âm lượng thì dùng **VideoPlayer.SetDirectAudioMute(0, true)**;
    2. Nếu xuất hiện lỗi “****AudioSampleProvider buffer overflow.****” tức là audio sample chạy trên scene quá nhiều gây tràn bộ nhớ → kiểm tra các object cần thiết bật audio, nếu không cần thiết thì tắt nó đi bằng cách edit nó trên **Inspector** hoặc dùng lệnh
        
        ```csharp
        VideoPlayer videoPlayer;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
        ```
        
- Lưu ý có 2 dạng **Material** và **Physic Material**
- Tip
    1. Nhấn F để focus vào đối tượng
    2. **Ctrl+Shift+F** để đưa camera đặt vào góc nhìn trực tiếp trên Scene
- ****`RequireComponent`**** tự động thêm các component phụ thuộc vào game object để tránh trường hợp lỗi khi setup sai
    
    Trong Unity, **`RequireComponent`** là một thuộc tính (attribute) được sử dụng để yêu cầu tự động thêm các thành phần (component) bổ sung vào một đối tượng khi nó được gắn vào trong cảnh (scene). Điều này giúp đảm bảo rằng đối tượng sẽ luôn có các thành phần cần thiết để hoạt động đúng cách.
    
    Ví dụ, giả sử chúng ta có một script có tên là **`PlayerController`** và nó yêu cầu đối tượng chứa nó phải có cả thành phần **`Rigidbody`** và **`Collider`**. Chúng ta có thể sử dụng **`RequireComponent`** để đảm bảo rằng các thành phần này sẽ tự động được thêm vào đối tượng khi nó được gắn vào trong cảnh.
    
    Dưới đây là một ví dụ về cách sử dụng **`RequireComponent`** trong Unity:
    
    ```csharp
    using UnityEngine;
    
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PlayerController : MonoBehaviour
    {
        // Code của script PlayerController
    }
    ```
    
    Trong ví dụ này, chúng ta đã áp dụng thuộc tính **`RequireComponent`** lên class **`PlayerController`** và chỉ định hai thành phần cần thiết là **`Rigidbody`** và **`Collider`**. Khi script **`PlayerController`** được gắn vào một đối tượng trong cảnh, Unity sẽ tự động thêm các thành phần này nếu chúng chưa tồn tại trên đối tượng đó.
    
    Điều này giúp đảm bảo rằng khi chúng ta sử dụng **`PlayerController`**, các thành phần **`Rigidbody`** và **`Collider`** sẽ luôn có sẵn và có thể được truy cập và sử dụng trong script mà không cần phải thêm chúng thủ công vào từng đối tượng.
    
- `typeof` trả về kiểu của một lớp trong System.Type
    
    
- Không thể thực hiện việc gán giá trị biến kiểu **GameObject** trong 1 hàm **Contructor** được
    
    Hàm **constructor** trong Unity được sử dụng để khởi tạo các đối tượng, gán giá trị ban đầu cho các thuộc tính và thực hiện các tác vụ khác liên quan đến việc khởi tạo.
    
- Sử dụng thuộc tính `activeSelf` của GameObject để kiểm tra xem một GameObject có đang active trên scene hay không
    
    
- Các thuộc tính dùng trong Inspector
    - Dùng `SerializeField` để hiển thị các biến trong Inspector.
        
        ```csharp
        [SerializeField] private int hiddenVar;
        ```
        
    - Dùng `HideInspector` để ẩn các biến public trong Inspector.
        
        ```csharp
        [HideInInspector] public float hiddenVar;
        ```
        
    - Dùng `Header` để đặt tên cho tiêu đề cho một số trường trong Inspector.
        
        ```csharp
        [Header("Drag & drop input here")]
        ```
        
    - Dùng `Space` **để t**hêm không gian trống giữa các biến trong Inspector.
        
        ```csharp
        [Space] public float jumpHeight;
        ```
        
    - Dùng `Range` để tạo một slider trong Inspector cho biến số.
        
        ```csharp
        [Range(0f, 100f)] public float health;
        ```
        
    - Dùng `Tooltip` để thêm mô tả ngắn gọn khi rê chuột qua biến trong Inspector.
        
        ```csharp
        [Tooltip("Tốc độ di chuyển của nhân vật")] public float speed;
        ```
        
    - `[ReadOnly] / [SerializeField, ReadOnly]` điều này không phải là một phần của Unity mặc định nhưng có thể được thực hiện thông qua custom attribute để hiển thị biến không thể chỉnh sửa trong Inspector
        
        ```csharp
        [ReadOnly] public float readOnlyVar;
        ```
        
    - `Multiline`và `TextArea` **d**ành cho chuỗi, để nhập văn bản nhiều dòng.
        
        ```csharp
        [Multiline] public string multilineString;
        
        [TextArea(3, 10)] public string textAreaString;
        ```
        
    - `EnumFlags` hiển thị enum dưới dạng tập hợp các lựa chọn.
        
        [EnumFlags] public MyEnum enumVar;
        
    - `ColorUsage` tùy chỉnh trình chọn màu cho biến màu.
        
        ```csharp
        [ColorUsage(false, true)] public Color colorVar;
        ```
        
    - `ContextMenu` thêm lệnh vào menu ngữ cảnh của đối tượng trong Inspector.
        
        ```csharp
        [ContextMenu("Do Something")] void DoSomething() { ... }
        ```
        
- PlayerController (Input player)
    - Khác nhau giữa `Rigidbody.AddForce()` và `Transform.Translate()`
        1. **Rigidbody.AddForce()**:
            - **Dựa vào vật lý**: Khi bạn sử dụng **`AddForce()`**, bạn đang thêm lực vào một **`Rigidbody`**, và hệ thống vật lý của Unity sẽ xử lý việc di chuyển đối tượng. Điều này có nghĩa là việc di chuyển sẽ tuân thủ các luật vật lý, như ma sát, trọng lực, và độ đàn hồi khi va chạm.
            - **Tương tác với môi trường**: Đối tượng có thể va chạm và tương tác với các đối tượng khác trong cảnh.
            - **Thời gian thực**: Dựa vào hệ thống vật lý thời gian thực.
            - **Ứng dụng**: Thường được sử dụng khi bạn muốn một đối tượng tương tác với môi trường theo cách tự nhiên và thực tế.
        2. **Transform.Translate()**:
            - **Không dựa vào vật lý**: Đây là một phép biến đổi toạ độ trực tiếp. Đối tượng sẽ di chuyển theo một vectơ đã cho mà không quan tâm đến vật lý. Ví dụ, nó sẽ không bị ảnh hưởng bởi trọng lực hoặc ma sát.
            - **Không tương tác với môi trường**: Đối tượng sẽ di chuyển qua các đối tượng khác nếu không có logic xử lý va chạm đặc biệt.
            - **Độc lập thời gian**: Di chuyển một khoảng cách cố định, không liên quan đến thời gian thực.
            - **Ứng dụng**: Thích hợp cho việc di chuyển các đối tượng không cần tương tác với môi trường hoặc khi bạn muốn kiểm soát chặt chẽ việc di chuyển mà không bị ảnh hưởng bởi vật lý.
        
        **Kết luận**:
        
        - Nếu bạn đang làm việc với một trò chơi dựa trên vật lý (ví dụ như một trò chơi giả lập lái xe), bạn nên sử dụng **`Rigidbody.AddForce()`**.
        - Nếu bạn chỉ muốn di chuyển một đối tượng trong không gian mà không cần tới vật lý, **`Transform.Translate()`** sẽ là lựa chọn tốt hơn.
    - **Collider** và **Collision**
        1. **Collider**:
            - `Collider` là một thành phần có thể thêm vào đối tượng trong Unity để định nghĩa một khu vực trong không gian mà đối tượng chiếm giữ, và nó dùng để phát hiện va chạm với các đối tượng khác.
            - Có nhiều loại `Collider` khác nhau như `Box Collider`, `Sphere Collider`, `Capsule Collider`, `Mesh Collider`, v.v.
            - `Collider` không giả định rằng đối tượng sẽ tương tác với các đối tượng khác theo cách thức của vật lý (như bật lại hoặc di chuyển), nó chỉ dùng để phát hiện va chạm. Để thực hiện các tương tác vật lý thực sự, một `Rigidbody` cần phải được thêm vào đối tượng.
        2. **Collision**:
            - `Collision` không phải là một thành phần mà bạn thêm vào một đối tượng. Thay vào đó, nó là một đối tượng được truyền vào các hàm callback vật lý như `OnCollisionEnter`, `OnCollisionStay`, và `OnCollisionExit`.
            - Khi hai đối tượng có `Collider` va chạm với nhau và ít nhất một trong chúng có một `Rigidbody`, các hàm callback này sẽ được gọi, và một đối tượng `Collision` sẽ được truyền vào như một tham số.
            - Đối tượng `Collision` cung cấp thông tin về va chạm, bao gồm các điểm va chạm, sức va chạm, đối tượng mà bạn va chạm với, và nhiều thông tin khác.
        
        Tóm lại:
        
        - **Collider**: Định nghĩa khu vực không gian mà một đối tượng chiếm giữ và được dùng để phát hiện va chạm.
        - **Collision**: Cung cấp thông tin về một sự kiện va chạm cụ thể giữa hai đối tượng trong Unity.
- Tag trong Unity là gì?
    
    Trong Unity, mỗi GameObject có thể được gán một tag. Tag là một từ khóa hoặc nhãn giúp xác định đối tượng mà không cần phải dựa vào tên của đối tượng hoặc thành phần khác. Tag được sử dụng để dễ dàng nhận biết và phân loại các GameObject trong một cảnh hoặc trên toàn bộ dự án.
    
    Chẳng hạn, bạn có thể có nhiều đối tượng trong cảnh của mình được gán tag "Enemy". Thay vì viết code để kiểm tra tên của mỗi GameObject khi bạn muốn kiểm tra xem một đối tượng có phải là kẻ thù không, bạn có thể sử dụng tag để làm điều đó một cách dễ dàng hơn.
    
    ```csharp
    // Giả sử bạn muốn kiểm tra xem đối tượng mà người chơi đã va chạm có phải là "Enemy" không
    void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra tag của GameObject mà người chơi va chạm
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Nếu GameObject có tag là "Enemy"
            Debug.Log("Player has collided with an enemy!");
        }
    }
    ```
    
- Thao tác các child-GameObject trong 1 parent-GameObject thì dùng transform
- [System.IO](http://System.IO) Thao tác với tệp input out put trong Unity
    - Cách tạo prefab trong Unity khi tồn tại trên scene bằng C#
        
        Để tạo một prefab trong Unity từ một đối tượng đã tồn tại trên scene, bạn có thể sử dụng Unity Editor hoặc viết một script C# để tự động hóa quá trình này. Tuy nhiên, cần lưu ý rằng, việc tạo và xử lý prefab chủ yếu được thực hiện thông qua Unity Editor, và các API liên quan đến prefab trong scripting thường dành cho các tác vụ như tạo instance của prefab hoặc làm việc với các instance đã tồn tại.
        
        Dưới đây là cách bạn có thể tạo prefab từ một đối tượng trên scene sử dụng Unity Editor:
        
        **2. Tạo Prefab Sử Dụng Script C#:**
        
        - Để tự động hóa việc tạo prefab, bạn có thể sử dụng `PrefabUtility`. Đây là một lớp trong Unity Editor API và chỉ hoạt động trong môi trường Editor.
        
        Ví dụ về script:
        
        ```csharp
        #if UNITY_EDITOR
        using UnityEngine;
        using UnityEditor;
        
        public class PrefabCreator : MonoBehaviour
        {
            public GameObject objectToPrefab; // Đặt đối tượng từ Inspector
            public string prefabPath = "Assets/Prefabs/MyPrefab.prefab"; // Đường dẫn lưu prefab
        
            public void CreatePrefab()
            {
                if (objectToPrefab == null)
                {
                    Debug.LogError("Object to prefab is null!");
                    return;
                }
        
                // Tạo prefab từ đối tượng
                PrefabUtility.SaveAsPrefabAsset(objectToPrefab, prefabPath);
                Debug.Log("Prefab created at " + prefabPath);
            }
        }
        #endif
        
        ```
        
        - Trong script trên, `objectToPrefab` là đối tượng bạn muốn chuyển đổi thành prefab.
        - `prefabPath` là đường dẫn trong thư mục "Assets" nơi prefab sẽ được lưu.
        - Sử dụng `PrefabUtility.SaveAsPrefabAsset` để tạo prefab.
        - Đoạn script này chỉ hoạt động trong môi trường Unity Editor do sử dụng `#if UNITY_EDITOR`.
            
            Nhớ rằng khi sử dụng các tính năng của Editor như `PrefabUtility`, bạn không thể sử dụng chúng trong build cuối cùng của game; chúng chỉ hoạt động trong Unity Editor.
            
    - Cách link các prefab có sẵn vào trong scriptable object
        
        Để tạo một `ScriptableObject` chứa một danh sách `GameObject` và tự động thêm tất cả các prefab từ một thư mục cụ thể vào danh sách này, bạn sẽ cần viết một script Editor trong Unity. Script này sẽ sử dụng các API của Unity Editor để tìm và thêm các prefab vào `ScriptableObject` bạn đã tạo.
        
        Dưới đây là các bước để thực hiện việc này:
        
        ### Bước 1: Định Nghĩa Lớp ScriptableObject
        
        Đầu tiên, bạn cần một lớp `ScriptableObject` có một `List<GameObject>`:
        
        ```csharp
        using UnityEngine;
        using System.Collections.Generic;
        
        [CreateAssetMenu(fileName = "BottleData", menuName = "Bottle/Create Bottle Data")]
        public class BottleData : ScriptableObject
        {
            public List<GameObject> bottles;
        }
        ```
        
        ### Bước 2: Tạo Script Tìm và Thêm Prefab
        
        Sau đó, tạo một script Editor để tìm tất cả prefab trong thư mục và thêm chúng vào `ScriptableObject`:
        
        ```csharp
        #if UNITY_EDITOR
        using UnityEngine;
        using UnityEditor;
        using System.Collections.Generic;
        using System.IO;
        
        public static class BottleDataCreator
        {
            [MenuItem("Bottle/Create Bottle Data From Prefabs")]
            public static void CreateBottleData()
            {
                // Tạo ScriptableObject mới
                BottleData bottleData = ScriptableObject.CreateInstance<BottleData>();
                bottleData.bottles = new List<GameObject>();
        
                // Tìm tất cả prefab trong thư mục
                string[] prefabGuids = AssetDatabase.FindAssets("t:GameObject", new[] { "Assets/bottleFolder" });
                foreach (string guid in prefabGuids)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                    if (prefab != null)
                    {
                        bottleData.bottles.Add(prefab);
                    }
                }
        
                // Lưu ScriptableObject
                string assetPathForData = "Assets/BottleData.asset";
                AssetDatabase.CreateAsset(bottleData, assetPathForData);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
        
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = bottleData;
            }
        }
        #endif
        ```
        
        Trong script này:
        
        - `BottleData` là lớp `ScriptableObject` bạn đã tạo.
        - `CreateBottleData` là phương thức tạo `ScriptableObject` và điền nó với các prefab từ thư mục chỉ định.
        - `AssetDatabase.FindAssets` được sử dụng để tìm tất cả GameObject trong thư mục "Assets/bottleFolder".
        - Mỗi prefab tìm được sẽ được thêm vào danh sách `bottles` của `BottleData`.
        
        ### Bước 3: Sử Dụng Script Để Tạo BottleData
        
        - Trong Unity Editor, sử dụng lệnh "Bottle/Create Bottle Data From Prefabs" từ menu để chạy script và tạo `BottleData`.
        - `BottleData` mới sẽ chứa tất cả prefab từ thư mục đã chỉ định.
        
        Lưu ý: Script này chỉ chạy được trong Unity Editor và không phải là phần của logic game trong build cuối cùng.
        
- Chỉnh chu khi làm sản phẩm game
    - Version game nên đạt ở góc trái

## 1. Tìm hiểu

- Content Size Fitter là gì trong Unity?
    
    Trong Unity, `Content Size Fitter` là một component UI được sử dụng trong hệ thống giao diện người dùng (UI) của Unity để tự động điều chỉnh kích thước của một `RectTransform` để phù hợp với kích thước của nội dung bên trong nó. Nói cách khác, `Content Size Fitter` cho phép một GameObject tự động thay đổi kích thước của mình để phù hợp với kích thước nội dung mà nó chứa, dựa trên các quy tắc cụ thể.
    
    **Các tính năng chính của `Content Size Fitter`:**
    
    1. **Horizontal Fit và Vertical Fit:**
        - `Content Size Fitter` cung cấp hai tùy chọn chính: `Horizontal Fit` và `Vertical Fit`.
        - Mỗi tùy chọn này có thể được thiết lập để "Unconstrained", "Preferred Size", hoặc "Min Size".
        - "Preferred Size" sẽ điều chỉnh kích thước của GameObject để phù hợp với kích thước tối ưu của nội dung nó chứa.
        - "Min Size" sẽ đảm bảo GameObject không nhỏ hơn kích thước tối thiểu của nội dung.
    2. **Làm Việc Cùng Layout Groups:**
        - `Content Size Fitter` thường được sử dụng cùng với các layout group như `VerticalLayoutGroup` hoặc `HorizontalLayoutGroup` để tạo ra các UI đáp ứng linh hoạt, tự điều chỉnh theo nội dung.
    3. **Ứng dụng Thực Tế:**
        - Thường được sử dụng trong các trường hợp như các danh sách nội dung có thể thay đổi kích thước, text boxes tự điều chỉnh theo nội dung văn bản, hoặc bất kỳ UI component nào cần phải thay đổi kích thước dựa trên nội dung động của nó.
    
    **Ví Dụ Sử Dụng:**
    Giả sử bạn có một text box chứa nội dung văn bản mà độ dài có thể thay đổi. Bạn không muốn text box này cố định kích thước mà muốn nó tự mở rộng hoặc thu nhỏ để phù hợp với nội dung. Bạn có thể gắn `Content Size Fitter` vào GameObject của text box và thiết lập các tùy chọn phù hợp để đảm bảo rằng kích thước của box luôn phù hợp với độ dài của văn bản.
    
- Scroll Rect Component là gì trong Unity?
