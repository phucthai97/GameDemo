# Unity Cheat Sheet

## Phụ lục
- [Cơ bản](#coban)
    - [Vector](#vector)
    - [MonoBehaviour](#monobehaviour)
        - [Awake()](#1.-Awake())
        - [OnEnable()](#onenable)
        - [Start()](#start)
        - [FixedUpdate()](#fixedupdate)
        - [Update()](#update)
        - [OnTrigger()](#ontrigger)
        - [OnCollision()](#oncolision)
        - [OnMouse()](#onmouse)
        - [Update()](#update)
        - [LateUpdate()](#lateupdate)
        - [OnRenderObject()](#onrenderobject)
        - [OnGUI()](#ongui)
        - [OnDisable()](#ondisable)
        - [OnDestroy()](#ondestroy)
    - [ScriptableObject](#scriptableobject)

## A. Cơ bản
### Vector
- Dùng để biểu diễn hướng và độ lớn trong không gian 2D hoặc 3D. Unity cung cấp một số loại vector khác nhau, phổ biến nhất là Vector2, Vector3, và Vector4.
- Các thuộc tính tĩnh của Vector3
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
- Các thuộc tính chung của Vector3
    - magnitude: Thuộc tính magnitude trả về độ dài của vectơ (tính bằng cách sử dụng định lý Pythagoras).
        ```csharp
        Vector3 position = new Vector3(3, 4, 0);
        float length = position.magnitude; // length = 5
        ```
    - normalized: Thuộc tính normalized trả về phiên bản chuẩn hóa của vectơ, có cùng hướng nhưng độ dài bằng 1.
        ```csharp
        Vector3 position = new Vector3(3, 4, 0);
        Vector3 normalizedPosition = position.normalized;
        // normalizedPosition sẽ là vectơ có cùng hướng với position nhưng độ dài bằng 1
        ``` 
### MonoBehaviour
- MonoBehaviour là một lớp cơ bản trong Unity và là lớp cốt lõi cho hầu hết các script trong môi trường phát triển trò chơi của Unity. Nó cung cấp một khung để xây dựng các hành vi tùy chỉnh vào các đối tượng trong game của bạn thông qua kịch bản (scripting
- https://docs.unity3d.com/uploads/Main/monobehaviour_flowchart.svg
- Vòng đời của một GameObject theo thứ tự sau:
  #### 1. Awake()
    - Được gọi ngay khi instance của script được tạo ra, trước mọi hàm Start() và cả khi GameObject mà script gắn vào chưa được kích hoạt (tức là enabled = false).
  #### 2. OnEnable()
    - Được gọi mỗi khi một script hoặc GameObject trở nên hoạt động (enabled). Đây là nơi lý tưởng để thiết lập các sự kiện, cập nhật trạng thái, hoặc thực hiện khởi tạo liên quan đến việc kích hoạt lại một đối tượng.
    - Ví dụ: Giả sử bạn có một hệ thống sự kiện trong game và bạn muốn đối tượng của mình lắng nghe một sự kiện cụ thể khi nó được kích hoạt. Bạn có thể sử dụng OnEnable() để đăng ký sự kiện và OnDisable() để hủy đăng ký, như sau:
        ```csharp
        public class EventListener : MonoBehaviour
        {
            void OnEnable()
            {
                EventManager.OnCustomEvent += HandleCustomEvent;
            }
        
            void OnDisable()
            {
                EventManager.OnCustomEvent -= HandleCustomEvent;
            }
        
            private void HandleCustomEvent()
            {
                // Xử lý sự kiện ở đây
                Debug.Log("Sự kiện đã được kích hoạt!");
            }
        }
        ```
         📌 Trong ví dụ này:
        - EventManager.OnCustomEvent: Là sự kiện mà bạn muốn lắng nghe. EventManager là một lớp tượng trưng cho hệ thống quản lý sự kiện trong game của bạn.
        - HandleCustomEvent(): Là phương thức được gọi khi sự kiện OnCustomEvent được kích hoạt.

        📌 Cách Hoạt Động:
        - Khi script hoặc GameObject trở nên hoạt động, OnEnable() sẽ được gọi và phương thức HandleCustomEvent() sẽ được đăng ký với sự kiện.
        - Khi script hoặc GameObject bị vô hiệu hóa, OnDisable() sẽ được gọi và phương thức HandleCustomEvent() sẽ được hủy đăng ký khỏi sự kiện, ngăn chặn rò rỉ tài nguyên hoặc lỗi không mong muốn.
        
        ❓Khi Nào Sử Dụng OnEnable()?
        - OnEnable() thích hợp để sử dụng trong các tình huống sau:
            - Đăng ký các sự kiện hoặc thông báo.
            - Khởi tạo lại trạng thái hoặc thông tin khi GameObject được kích hoạt lại.
            - Tải hoặc cập nhật dữ liệu mà chỉ cần khi đối tượng hoạt động.
            - Sử dụng OnEnable() và OnDisable() một cách hiệu quả giúp quản lý tài nguyên và tình trạng của đối tượng một cách hiệu quả, đồng thời ngăn chặn lỗi và tối ưu hóa hiệu suất.
  #### 3. Start()
    - Được gọi ngay trước khung hình đầu tiên mà Update() chạy, nhưng sau tất cả các hàm Awake(). Điều quan trọng là Start() chỉ được gọi nếu script đang được kích hoạt
  #### 4. FixedUpdate()
    - Physics Update trong Unity được quản lý chủ yếu thông qua hàm FixedUpdate(). Đây là nơi bạn thực hiện các tính toán liên quan đến vật lý, như di chuyển đối tượng, áp dụng lực hoặc torque, vì FixedUpdate() chạy ổn định và độc lập với tốc độ khung hình, làm cho nó phù hợp cho hệ thống vật lý.
    - Ví dụ: Giả sử bạn muốn tạo một đối tượng có thể di chuyển theo các phím mũi tên trên bàn phím. Đầu tiên, đối tượng của bạn cần có một Rigidbody component để Unity có thể áp dụng vật lý lên nó.
        ```csharp
        public class PlayerController : MonoBehaviour
        {
            public float speed = 10.0f;
            private Rigidbody rb;
            private Vector3 movement;
        
            void Start()
            {
                rb = GetComponent<Rigidbody>();
            }

            void Update()
            {
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");
        
                movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            }
        
            void FixedUpdate()
            {
                rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
            }
        }
        ```
        📌 Trong ví dụ này:
        - speed: Đây là tốc độ mà đối tượng sẽ di chuyển.
        - rb: Là một tham chiếu đến Rigidbody component của đối tượng.
        - movement: Một Vector3 lưu trữ hướng di chuyển dựa trên đầu vào từ người chơi.

        📌 Cách Hoạt Động:
        - Trong hàm Update(), chúng ta lấy đầu vào từ bàn phím thông qua Input.GetAxis(), cho phép di chuyển theo trục ngang và dọc.
        - Trong FixedUpdate(), chúng ta di chuyển Rigidbody bằng cách sử dụng MovePosition(). Time.fixedDeltaTime đảm bảo rằng di chuyển không phụ thuộc vào số lần FixedUpdate() được gọi mỗi giây.
  #### 5. Update()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.
  #### 6. OnTrigger(), OnCollision()
    - Dùng để xử lý va chạm và kích hoạt (trigger) giữa các GameObject.Bao gồm các hàm OnCollisionEnter(), OnCollisionExit(), OnCollisionStay(), OnTriggerEnter(), OnTriggerExit(), OnTriggerStay()
        - OnCollisionEnter() Được gọi khi GameObject có Rigidbody va chạm với một GameObject khác.
            ```csharp
            void OnCollisionEnter(Collision collision)
            {
            if (collision.gameObject.tag == "Enemy")
                {
                    Debug.Log("Va chạm với kẻ địch!");
                }
            }
            ```
        - OnTriggerStay() Được gọi mỗi khung hình mà Collider của GameObject (được đánh dấu là Trigger) vẫn chạm vào Collider của GameObject khác.
            ```csharp
            void OnTriggerStay(Collider other)
            {
                if (other.tag == "SafeZone")
                {
                    Debug.Log("Đang ở trong khu vực an toàn!");
                }
            }
            ```
  #### 7. OnMouse()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.Bao gồm các hàm như:
        ```cshar
        OnMouseDown(): Được gọi khi chuột nhấp vào Collider của GameObject.
        OnMouseUp() Được gọi khi nút chuột được thả ra sau khi nhấn trên Collider của GameObject.
        OnMouseOver() Được gọi mỗi khung hình khi chuột di chuyển trên Collider của GameObject.
        OnMouseEnter() Được gọi một lần khi chuột bắt đầu di chuyển trên Collider của GameObject.
        OnMouseExit() Được gọi khi chuột rời khỏi Collider của GameObject.
        ```
    - Ví dụ:
        ```csharp
        void OnMouseDown()
        {
            Debug.Log("GameObject đã được nhấp chuột!");
            // Thực hiện các hành động, ví dụ: chọn đối tượng, mở menu, v.v.
        }
        ```
    - Lưu ý
        - Các hàm OnMouse... chỉ hoạt động nếu GameObject có một Collider.
        - Trong một số trường hợp, việc sử dụng hệ thống sự kiện chuột của Unity UI (như IPointerClickHandler, IPointerEnterHandler, v.v.) có thể phù hợp hơn, đặc biệt là khi làm việc với giao diện người dùng.
        - Cần cẩn thận khi sử dụng các hàm này với ứng dụng di động, vì chúng chủ yếu được thiết kế cho chuột và không luôn tương thích hoàn hảo với cảm ứng.
  #### 8. Update()
    - Được gọi mỗi khung hình. Nơi xử lý phần lớn logic trò chơi, kiểm tra đầu vào từ người chơi, chuyển động, v.v.
  #### 9. LateUpdate()
    - Được gọi ngay sau Update() trong mỗi khung hình. Thường được dùng cho các hành động cần thực hiện sau khi tất cả các lệnh Update() đã chạy, như điều chỉnh camera.
  #### 10. OnRenderObject()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.
  #### 11. OnGUI()
    - Được sử dụng để tạo giao diện người dùng (GUI) truyền thống. Nó chạy nhiều lần trong một khung hình (frame) và thích hợp cho các tác vụ như vẽ nút, hộp thoại, hoặc các thông tin tương tác trực tiếp. Tuy nhiên, cần lưu ý rằng OnGUI() có thể ảnh hưởng đến hiệu suất nếu không được sử dụng cẩn thận. Trong Unity hiện đại, nó thường được thay thế bằng hệ thống UI dựa trên Canvas.
    - Ví dụ: Tạo nút và hộp thoại
        ```csharp
        void OnGUI()
        {
            // Tạo một nút
            if (GUI.Button(new Rect(10, 10, 100, 50), "Nhấn vào đây"))
            {
                Debug.Log("Nút đã được nhấn!");
                // Thực hiện hành động khi nút được nhấn
            }
        
            // Hiển thị hộp thoại với văn bản
            GUI.Box(new Rect(10, 70, 100, 50), "Đây là hộp thoại");
            
            // Hiển thị văn bản
            GUI.Label(new Rect(10, 130, 200, 20), "Chào mừng đến với trò chơi của chúng tôi!");
        }
        ```
        📌 Lưu Ý Khi Sử Dụng
        - Hiệu Suất: OnGUI() có thể ảnh hưởng tiêu cực đến hiệu suất, đặc biệt nếu có nhiều phần tử GUI hoặc logic phức tạp.
        - Phong Cách: Bạn có thể tùy chỉnh phong cách của GUI bằng cách sử dụng GUIStyle.
        - Thay Thế: Trong các dự án Unity mới, khuyến nghị sử dụng hệ thống UI dựa trên Canvas cho giao diện người dùng, vì nó linh hoạt và hiệu quả hơn.
        - Tương Thích: OnGUI() thích hợp cho các công cụ phát triển nhanh hoặc debug, nhưng không phải là lựa chọn tối ưu cho giao diện người dùng cuối cùng trong game.
  #### 12. OnDisable()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác
  #### 12. OnDestroy()
    - Được gọi khi một script hoặc GameObject sắp bị hủy. Đây là thời điểm thích hợp để thực hiện dọn dẹp sâu hơn, như hủy các Coroutine, đóng các file, hoặc giải phóng tài nguyên đã cấp phát.
        ```csharp
        void OnDestroy()
        {
            Debug.Log("Script hoặc GameObject sắp bị hủy");
            // Giải phóng tài nguyên, lưu trạng thái, v.v.
            // Hủy các Coroutine, đóng các kết nối mạng hoặc file
        }
        ```
        📌 Lưu Ý Khi Sử Dụng
        - Thứ Tự Gọi: OnDisable() được gọi trước OnDestroy(). Nếu GameObject bị vô hiệu hóa trước khi bị hủy, cả hai hàm này đều sẽ được gọi.
        - Đảm Bảo Dọn Dẹp: Việc sử dụng đúng cách OnDisable() và OnDestroy() giúp tránh rò rỉ tài nguyên và lỗi, đồng thời duy trì quy trình làm việc ổn định và hiệu quả.
        - Sự Khác Biệt: OnDisable() có thể được gọi nhiều lần khi script hoặc GameObject được kích hoạt và vô hiệu hóa, trong khi OnDestroy() chỉ được gọi một lần khi đối tượng sắp bị hủy vĩnh viễn.

### ScriptableObject
- ScriptableObject trong Unity là một lớp đặc biệt dùng để lưu trữ dữ liệu không phụ thuộc vào một đối tượng cụ thể nào trong cảnh (scene) của bạn. Khác với các đối tượng thông thường (được kế thừa từ MonoBehaviour), ScriptableObject cho phép bạn tạo ra các tài nguyên có thể tái sử dụng, lưu trữ dữ liệu trong trò chơi mà không cần phải gắn vào một GameObject cụ thể.
- Đặc Điểm của ScriptableObject:
    - Tối Ưu Hóa Bộ Nhớ: ScriptableObject giúp giảm việc sử dụng bộ nhớ không cần thiết, vì bạn không cần phải tạo ra nhiều bản sao của cùng một dữ liệu.
    - Tái Sử Dụng: Dễ dàng tái sử dụng cho nhiều đối tượng khác nhau trong trò chơi của bạn.
    - Quản Lý Dữ Liệu: Tốt cho việc quản lý cấu hình, thiết lập, hoặc bất kỳ loại dữ liệu không đổi nào khác.
    - Lưu Trữ Dữ Liệu: Khả năng lưu trữ thông tin ngay cả khi không chạy trò chơi (không giống như các biến thông thường trong MonoBehaviour).
- Giả sử bạn đang phát triển một trò chơi và muốn có một cách để quản lý thông tin về các loại vũ khí. Thay vì tạo ra nhiều bản sao của thông tin vũ khí trên mỗi đối tượng sử dụng vũ khí đó, bạn có thể dùng ScriptableObject để lưu trữ dữ liệu này.
- Bước 1: Tạo Lớp ScriptableObject:
    ```csharp
    using UnityEngine;
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
    public class Weapon : ScriptableObject
    {
        public string weaponName;
        public int damage;
        public Sprite icon;
        // Các thuộc tính khác...
    }
    ```
- Trong đoạn code trên, bạn tạo một lớp Weapon kế thừa từ ScriptableObject. CreateAssetMenu cho phép bạn tạo mới Weapon từ Unity Editor.
- Bước 2: Sử Dụng ScriptableObject trong Game:
Sau khi tạo Weapon asset, bạn có thể gán nó cho các đối tượng trong trò chơi của mình, ví dụ như các nhân vật hoặc kẻ thù.
    ```cshap
    public class Character : MonoBehaviour
    {
        public Weapon equippedWeapon;
    
        void DealDamage()
        {
            Debug.Log("Gây " + equippedWeapon.damage + " điểm sát thương.");
            // Thực hiện hành động gây sát thương
        }
    }
    ```
- Trong ví dụ này, mỗi Character có thể được trang bị một Weapon, và bạn có thể dễ dàng thay đổi vũ khí cho nhân vật mà không cần thay đổi toàn bộ logic hoặc dữ liệu của nhân vật.
- Khi Nào Nên Sử Dụng ScriptableObject?
    - Khi bạn cần một cách để lưu trữ và quản lý dữ liệu mà không phụ thuộc vào cảnh hoặc đối tượng cụ thể.
    - Đối với dữ liệu cấu hình, thiết lập hoặc bất kỳ loại dữ liệu chung nào mà bạn muốn tái sử dụng qua nhiều cảnh hoặc đối tượng.
    - Khi bạn muốn tránh việc tạo ra nhiều bản sao không cần



