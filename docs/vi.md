# Effects Engine — hiệu ứng hình ảnh động cho các cảnh

Effects Engine cho phép kịch bản phát một hiệu ứng hình ảnh ngắn đè lên cảnh: một tia chớp, khói, tia lửa, quầng sáng ma thuật. Một hiệu ứng là một cảnh Godot nhỏ mà mod đóng gói trong thư mục `effects/` của mình; kịch bản gọi nó theo tên, và nó giữ nguyên trên màn hình cho tới khi tự kết thúc hoặc hộp thoại kết thúc.

Đây là mod engine, tự nó chẳng có gì để xem. Các game hiện được kèm theo chưa đóng gói cảnh hiệu ứng nào, nên khi chơi bạn sẽ không gặp hiệu ứng; mod này dành cho những tác giả muốn thêm một hiệu ứng.

## Hiệu ứng xuất hiện thế nào

1. Một dòng kịch bản `effect <tên>` chạy.
2. Effects Engine tìm cảnh `effects/<tên>.tscn` trong các mod đã tải.
3. Hiệu ứng được đặt lên trên bức tranh cảnh — trên nền và các vật thể của nó, dưới hộp thoại — và phát.
4. Khi hộp thoại đã gọi nó kết thúc, hiệu ứng được dỡ bỏ, trừ khi nó đã tự dỡ trước đó.

Kịch bản không chờ hiệu ứng: dòng kế tiếp hiện ngay.

## Cho tác giả: thêm một hiệu ứng

Đặt cảnh hiệu ứng vào mod của bạn:

```filestree
my_mod/
└── effects/
    └── sparks.tscn
```

Gọi nó từ kịch bản:

```scenario
text "Ngọn đèn lập lòe."
effect sparks
text "Có thứ gì đó cựa mình trong bóng tối."
end
```

**Mod phải là thư mục.** Cảnh Godot chỉ nạp được từ mod được cài dưới dạng thư mục. Mod phát hành dưới dạng lưu trữ `.zip` sẽ bị bỏ qua hiệu ứng, kèm cảnh báo trong nhật ký.

### Những viên gạch cho cảnh hiệu ứng

Effects Engine cung cấp ba cách hoạt ảnh các khung hình bên trong cảnh hiệu ứng của bạn:

- **Bộ khung hình có hòa trộn** — một node gắn script `EffectDataSet`, một danh sách ảnh khung hình và một `AnimationController` hòa khung hình này vào khung hình kế. Tốc độ và thời gian hòa đặt trong inspector; khung hình phát một lần rồi hiệu ứng tự dỡ.
- **Bảng sprite trong shader** — `EffectAnimator` đưa một `TextureRect` với vật liệu bảng khung đi qua một số khung định trước trong một khoảng thời gian định trước.
- **Bảng sprite không dùng shader** — `EffectAnimNode` cuộn qua một bảng sprite nằm ngang (rộng khung = rộng bảng / số khung) và tự dỡ khi hết. Cũng có thể tạo từ code bằng `EffectAnimNode.SpawnOn(target, sheet, frameCount, seconds)`.

Bộ khung hình và bảng sprite thuần code sẽ giãn ra lấp đầy cảnh, nên những khung vẽ đúng kích thước cảnh sẽ khớp với nền.

### Hiệu ứng đầu tiên không cần code

Hiệu ứng đơn giản nhất là một cảnh có gốc là `TextureRect` chứa một bức ảnh giãn kín màn hình — ví dụ một quầng sáng mờ. Nó hiện lên theo `effect <tên>` và biến mất khi hộp thoại kết thúc.

## Khi có trục trặc

1. **Không có gì hiện lên và nhật ký bảo không tìm thấy hiệu ứng.** Tệp phải nằm ở `effects/<tên>.tscn` trong một mod thuộc game, và tên trong kịch bản phải khớp.
2. **Hiệu ứng chạy lúc phát triển nhưng không chạy với người chơi.** Mod được phát hành dưới dạng `.zip`; hãy phát hành dưới dạng thư mục.
3. **Hiệu ứng không chịu biến mất.** Nó ở lại cho tới khi hộp thoại của nó kết thúc; hãy kết thúc hộp thoại, hoặc dựng hiệu ứng từ các viên gạch bộ khung hình hay bảng sprite — chúng tự dỡ khi phát xong.
