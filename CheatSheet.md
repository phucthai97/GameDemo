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
  #### Awake()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.
  #### OnEnable()
    - Được gọi khi GameObject hoặc script được kích hoạt. Cũng được gọi sau Awake() nếu script đang kích hoạt.
  #### Start()
    - được gọi ngay trước khung hình đầu tiên mà Update() chạy, nhưng sau tất cả các hàm Awake(). Điều quan trọng là Start() chỉ được gọi nếu script đang được kích hoạt
    - <span style="color:red;">abc</span>
  #### FixedUpdate()
    - Được gọi mỗi khung hình cố định (mặc định là 50 lần mỗi giây), không phụ thuộc vào tốc độ khung hình. Sử dụng cho logic vật lý như di chuyển hoặc áp dụng lực.
  #### Update()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.
  #### OnTrigger(), OnCollision()
    - Dùng để xử lý va chạm và kích hoạt (trigger) giữa các GameObject.
        - OnCollisionEnter()
            ```csharp
            void OnCollisionEnter(Collision collision)
            {
            if (collision.gameObject.tag == "Enemy")
                {
                    Debug.Log("Va chạm với kẻ địch!");
                }
            }
            ```
  #### OnMouse()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.
  #### Update()
    - Được gọi mỗi khung hình. Nơi xử lý phần lớn logic trò chơi, kiểm tra đầu vào từ người chơi, chuyển động, v.v.
  #### LateUpdate()
    - Được gọi ngay sau Update() trong mỗi khung hình. Thường được dùng cho các hành động cần thực hiện sau khi tất cả các lệnh Update() đã chạy, như điều chỉnh camera.
  #### OnRenderObject()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.
  #### OnGUI()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.
  #### OnDisable()
    - Được gọi khi script instance được tải, ngay cả khi script bị vô hiệu hóa. Thường được dùng để khởi tạo các biến hoặc thiết lập tham chiếu đến các thành phần khác.













