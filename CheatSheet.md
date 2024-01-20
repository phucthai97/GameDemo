# Unity Cheat Sheet

## Phụ lục
- [Cơ bản](#coban)
    - [MonoBehaviour](#monobehaviour)
        - [Awake()](#awake)
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
    - []
    - 
    - 



## Cơ bản
### MonoBehaviour
- MonoBehaviour là một lớp cơ bản trong Unity và là lớp cốt lõi cho hầu hết các script trong môi trường phát triển trò chơi của Unity. Nó cung cấp một khung để xây dựng các hành vi tùy chỉnh vào các đối tượng trong game của bạn thông qua kịch bản (scripting
- https://docs.unity3d.com/uploads/Main/monobehaviour_flowchart.svg
- Vòng đời của một GameObject theo thứ tự sau:
  #### 1. Awake()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.
  #### 2. OnEnable()
    - Được gọi khi GameObject hoặc script được kích hoạt. Cũng được gọi sau Awake() nếu script đang kích hoạt.
  #### 3. Start()
    - được gọi ngay trước khung hình đầu tiên mà Update() chạy, nhưng sau tất cả các hàm Awake(). Điều quan trọng là Start() chỉ được gọi nếu script đang được kích hoạt
    - <span style="color:red;">abc</span>
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
    - Trong ví dụ này:
        - speed: Đây là tốc độ mà đối tượng sẽ di chuyển.
        - rb: Là một tham chiếu đến Rigidbody component của đối tượng.
        - movement: Một Vector3 lưu trữ hướng di chuyển dựa trên đầu vào từ người chơi.
    Cách Hoạt Động:
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
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.
  #### 8. Update()
    - Được gọi mỗi khung hình. Nơi xử lý phần lớn logic trò chơi, kiểm tra đầu vào từ người chơi, chuyển động, v.v.
  #### 9. LateUpdate()
    - Được gọi ngay sau Update() trong mỗi khung hình. Thường được dùng cho các hành động cần thực hiện sau khi tất cả các lệnh Update() đã chạy, như điều chỉnh camera.
  #### 10. OnRenderObject()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.
  #### 11. OnGUI()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.
  #### 12. OnDisable()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.






