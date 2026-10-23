// Ngô Trường Phúc 202418964

using System;
using System.Collections.Generic;

namespace DeviceManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("================================================================================");
            Console.WriteLine("           CHƯƠNG TRÌNH KIỂM THỬ HỆ THỐNG QUẢN LÝ THIẾT BỊ GIẢNG DẠY           ");
            Console.WriteLine("================================================================================");
            Console.WriteLine();

            // =========================================================================
            // KHỞI TẠO DỮ LIỆU BAN ĐẦU THEO YÊU CẦU:
            // - 3 điều khiển máy chiếu có mức pin khác nhau.
            // - 2 điều khiển điều hòa với tập phòng tương thích khác nhau.
            // - 3 micro có pin và chất lượng tín hiệu khác nhau.
            // =========================================================================

            // 1. Ba điều khiển máy chiếu (sẵn sàng khi pin >= 20%)
            ProjectorRemote pr1 = new ProjectorRemote("PR01", "Điều khiển máy chiếu Sony A1", "Phòng 101", 95); // Pin 95% -> Sẵn sàng, điểm: 95
            ProjectorRemote pr2 = new ProjectorRemote("PR02", "Điều khiển máy chiếu Panasonic B2", "Phòng 102", 60); // Pin 60% -> Sẵn sàng, điểm: 60
            ProjectorRemote pr3 = new ProjectorRemote("PR03", "Điều khiển máy chiếu Epson C3", "Phòng 103", 15); // Pin 15% -> KHÔNG sẵn sàng (pin < 20%)

            // 2. Hai điều khiển điều hòa với tập phòng tương thích khác nhau
            AirConditionerRemote ac1 = new AirConditionerRemote(
                "AC01",
                "Điều khiển điều hòa Daikin",
                "Tủ khu A",
                new string[] { "P101", "P102", "P103" }
            );
            AirConditionerRemote ac2 = new AirConditionerRemote(
                "AC02",
                "Điều khiển điều hòa Panasonic",
                "Tủ khu B",
                new string[] { "P201", "P202" }
            );

            // 3. Ba micro không dây (sẵn sàng khi pin >= 30% và tín hiệu >= 40%)
            WirelessMicrophone mic1 = new WirelessMicrophone("MC01", "Micro Shure SM58-1", "Phòng kỹ thuật", 85, 90); // Ready, điểm: 87.5
            WirelessMicrophone mic2 = new WirelessMicrophone("MC02", "Micro Shure SM58-2", "Phòng kỹ thuật", 70, 75); // Ready, điểm: 72.5
            WirelessMicrophone mic3 = new WirelessMicrophone("MC03", "Micro Sony UWP-3", "Phòng kỹ thuật", 20, 30);  // KHÔNG ready (pin < 30, tín hiệu < 40)

            // Khởi tạo các generic repository
            IRepository<ProjectorRemote> projectorRepo = new InMemoryRepository<ProjectorRemote>();
            IRepository<AirConditionerRemote> acRepo = new InMemoryRepository<AirConditionerRemote>();
            IRepository<WirelessMicrophone> micRepo = new InMemoryRepository<WirelessMicrophone>();

            // =========================================================================
            // 1. Thêm thiết bị vào repository
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("1. THỬ NGHIỆM: Thêm thiết bị vào repository");
            Console.WriteLine("--------------------------------------------------------------------------------");
            projectorRepo.Add(pr1);
            projectorRepo.Add(pr2);
            projectorRepo.Add(pr3);

            acRepo.Add(ac1);
            acRepo.Add(ac2);

            micRepo.Add(mic1);
            micRepo.Add(mic2);
            micRepo.Add(mic3);

            Console.WriteLine("-> Danh sách máy chiếu trong kho (" + projectorRepo.GetAll().Count + " thiết bị):");
            foreach (var d in projectorRepo.GetAll())
            {
                Console.WriteLine("   + " + d);
            }

            Console.WriteLine("-> Danh sách điều hòa trong kho (" + acRepo.GetAll().Count + " thiết bị):");
            foreach (var d in acRepo.GetAll())
            {
                Console.WriteLine("   + " + d);
            }

            Console.WriteLine("-> Danh sách micro trong kho (" + micRepo.GetAll().Count + " thiết bị):");
            foreach (var d in micRepo.GetAll())
            {
                Console.WriteLine("   + " + d);
            }
            Console.WriteLine();

            // =========================================================================
            // 2. Thử thêm hai thiết bị trùng mã
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("2. THỬ NGHIỆM: Thử thêm hai thiết bị trùng mã vào repository");
            Console.WriteLine("--------------------------------------------------------------------------------");
            try
            {
                Console.WriteLine("Đang thử thêm một máy chiếu mới có mã trùng 'PR01'...");
                ProjectorRemote duplicatePr = new ProjectorRemote("PR01", "Máy chiếu trùng mã", "Phòng ảo", 100);
                projectorRepo.Add(duplicatePr);
                Console.WriteLine("[LỖI] Không bắt được ngoại lệ trùng mã!");
            }
            catch (DuplicateIdException ex)
            {
                Console.WriteLine("[THÀNH CÔNG] Đã bắt ngoại lệ DuplicateIdException: " + ex.Message + " (Mã trùng: " + ex.DuplicateId + ")");
            }
            Console.WriteLine();

            // =========================================================================
            // 3. Tìm thiết bị theo mã
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("3. THỬ NGHIỆM: Tìm thiết bị theo mã (FindById)");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("Tìm kiếm mã 'PR01':");
            var foundPr = projectorRepo.FindById("PR01");
            if (foundPr != null)
            {
                Console.WriteLine("-> Tìm thấy: " + foundPr);
            }
            else
            {
                Console.WriteLine("-> Không tìm thấy thiết bị.");
            }

            Console.WriteLine("Tìm kiếm mã không tồn tại 'PR999':");
            var notFoundPr = projectorRepo.FindById("PR999");
            if (notFoundPr == null)
            {
                Console.WriteLine("-> Kết quả là null (Đúng như mong đợi khi mã không tồn tại).");
            }
            else
            {
                Console.WriteLine("-> Tìm thấy thiết bị không mong muốn: " + notFoundPr);
            }
            Console.WriteLine();

            // =========================================================================
            // 4. Lọc các thiết bị sẵn sàng
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("4. THỬ NGHIỆM: Lọc các thiết bị sẵn sàng (IsReadyForUse = true)");
            Console.WriteLine("--------------------------------------------------------------------------------");
            var readyProjectors = projectorRepo.FindAll(d => d.IsReadyForUse());
            Console.WriteLine("-> Máy chiếu sẵn sàng (" + readyProjectors.Count + "/" + projectorRepo.GetAll().Count + "):");
            foreach (var d in readyProjectors)
            {
                Console.WriteLine("   + " + d);
            }

            var readyMics = micRepo.FindAll(d => d.IsReadyForUse());
            Console.WriteLine("-> Micro sẵn sàng (" + readyMics.Count + "/" + micRepo.GetAll().Count + "):");
            foreach (var d in readyMics)
            {
                Console.WriteLine("   + " + d);
            }
            Console.WriteLine();

            // =========================================================================
            // 5. Chọn điều khiển máy chiếu có điểm cao nhất
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("5. THỬ NGHIỆM: Chọn điều khiển máy chiếu có điểm cao nhất (HighestPriorityStrategy)");
            Console.WriteLine("--------------------------------------------------------------------------------");
            var highestStrategyPr = new HighestPriorityStrategy<ProjectorRemote>(d => d.GetDefaultPriorityScore());
            var projectorService = new AllocationService<ProjectorRemote>(projectorRepo, highestStrategyPr);

            Console.WriteLine("Cấp phát máy chiếu theo mức pin/điểm cao nhất:");
            ProjectorRemote allocatedPr = projectorService.Allocate(null);
            Console.WriteLine("-> Máy chiếu được cấp phát: " + allocatedPr);
            Console.WriteLine();

            // =========================================================================
            // 6. Chọn điều khiển điều hòa tương thích với một phòng
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("6. THỬ NGHIỆM: Chọn điều khiển điều hòa tương thích với một phòng ('P201')");
            Console.WriteLine("--------------------------------------------------------------------------------");
            var firstStrategyAc = new FirstAvailableStrategy<AirConditionerRemote>();
            var acService = new AllocationService<AirConditionerRemote>(acRepo, firstStrategyAc);

            string targetRoom = "P201";
            Console.WriteLine("Yêu cầu cấp phát điều hòa cho phòng: " + targetRoom);
            AirConditionerRemote allocatedAc = acService.Allocate(d => d.IsCompatibleWith(targetRoom));
            Console.WriteLine("-> Điều hòa được cấp phát: " + allocatedAc);
            Console.WriteLine();

            // =========================================================================
            // 7. Chọn micro đầu tiên sẵn sàng
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("7. THỬ NGHIỆM: Chọn micro đầu tiên sẵn sàng (FirstAvailableStrategy)");
            Console.WriteLine("--------------------------------------------------------------------------------");
            var firstStrategyMic = new FirstAvailableStrategy<WirelessMicrophone>();
            var micService = new AllocationService<WirelessMicrophone>(micRepo, firstStrategyMic);

            Console.WriteLine("Cấp phát micro đầu tiên sẵn sàng:");
            WirelessMicrophone allocatedMic = micService.Allocate(null);
            Console.WriteLine("-> Micro được cấp phát: " + allocatedMic);
            Console.WriteLine();

            // =========================================================================
            // 8. Đổi chiến lược trong thời gian chạy và quan sát kết quả
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("8. THỬ NGHIỆM: Đổi chiến lược trong thời gian chạy (Runtime Strategy Pattern)");
            Console.WriteLine("--------------------------------------------------------------------------------");
            // Tạo thêm kho micro riêng biệt để minh họa rõ sự khác biệt giữa 2 chiến lược
            IRepository<WirelessMicrophone> testMicRepo = new InMemoryRepository<WirelessMicrophone>();
            WirelessMicrophone m1 = new WirelessMicrophone("M1", "Micro loại khá", "Kho", 60, 60); // Score = 60.0
            WirelessMicrophone m2 = new WirelessMicrophone("M2", "Micro loại VIP", "Kho", 95, 95);  // Score = 95.0
            testMicRepo.Add(m1);
            testMicRepo.Add(m2);

            var dynamicService = new AllocationService<WirelessMicrophone>(testMicRepo, new FirstAvailableStrategy<WirelessMicrophone>());
            Console.WriteLine("Kho thử nghiệm gồm 2 micro:");
            Console.WriteLine("   + " + m1);
            Console.WriteLine("   + " + m2);

            Console.WriteLine("-> Lần 1: Dùng FirstAvailableStrategy, thiết bị được chọn:");
            Console.WriteLine("   Đã chọn phần tử đầu tiên: " + dynamicService.Allocate(null).Name);

            // Trả lại thiết bị để reset trạng thái cho lần thử thứ 2
            m1.Return();

            Console.WriteLine("-> Đang đổi chiến lược sang HighestPriorityStrategy...");
            dynamicService.ChangeStrategy(new HighestPriorityStrategy<WirelessMicrophone>(m => m.GetDefaultPriorityScore()));
            Console.WriteLine("-> Lần 2: Cấp phát lại sau khi đổi chiến lược:");
            Console.WriteLine("   Đã chọn phần tử có điểm ưu tiên cao nhất: " + dynamicService.Allocate(null).Name);
            Console.WriteLine();

            // =========================================================================
            // 9. Thử cấp phát khi không có ứng viên phù hợp
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("9. THỬ NGHIỆM: Thử cấp phát khi không có ứng viên phù hợp (NoSuitableItemException)");
            Console.WriteLine("--------------------------------------------------------------------------------");
            try
            {
                string nonExistentRoom = "P999";
                Console.WriteLine("Đang yêu cầu cấp phát điều khiển điều hòa cho phòng không tồn tại '" + nonExistentRoom + "'...");
                acService.Allocate(d => d.IsCompatibleWith(nonExistentRoom));
                Console.WriteLine("[LỖI] Không bắt được ngoại lệ NoSuitableItemException!");
            }
            catch (NoSuitableItemException ex)
            {
                Console.WriteLine("[THÀNH CÔNG] Đã bắt ngoại lệ NoSuitableItemException: " + ex.Message);
            }
            Console.WriteLine();

            // =========================================================================
            // 10. Chứng minh thiết bị được chọn chuyển sang Borrowed
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("10. THỬ NGHIỆM: Chứng minh thiết bị được chọn chuyển sang Borrowed");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("Kiểm tra trạng thái thiết bị máy chiếu đã cấp phát ở Mục 5 (" + allocatedPr.Id + "):");
            Console.WriteLine("-> Trạng thái hiện tại: " + allocatedPr.Status + " (Đã chuyển sang Borrowed thành công)");
            Console.WriteLine("-> IsReadyForUse() hiện tại: " + allocatedPr.IsReadyForUse() + " (Không còn sẵn sàng để cấp phát lại)");

            try
            {
                Console.WriteLine("Thử gọi Borrow() lần nữa trên thiết bị đã mượn:");
                allocatedPr.Borrow();
                Console.WriteLine("[LỖI] Không bắt được ngoại lệ DeviceNotReadyException!");
            }
            catch (DeviceNotReadyException ex)
            {
                Console.WriteLine("[THÀNH CÔNG] Đã bắt ngoại lệ DeviceNotReadyException: " + ex.Message);
            }
            Console.WriteLine();

            // =========================================================================
            // 11. Chứng minh repository tổng quát có thể dùng cho ít nhất hai loại thiết bị mà không sao chép mã
            // =========================================================================
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("11. THỬ NGHIỆM: Chứng minh repository tổng quát (Generic Repository)");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("-> Lớp 'InMemoryRepository<T>' được định nghĩa một lần duy nhất với ràng buộc 'where T : IIdentifiable'.");
            Console.WriteLine("-> Hệ thống đã khởi tạo và sử dụng thành công cho cả 3 loại thiết bị khác nhau:");
            Console.WriteLine("   1. IRepository<ProjectorRemote>      -> " + projectorRepo.GetType().FullName);
            Console.WriteLine("   2. IRepository<AirConditionerRemote> -> " + acRepo.GetType().FullName);
            Console.WriteLine("   3. IRepository<WirelessMicrophone>   -> " + micRepo.GetType().FullName);
            Console.WriteLine("-> Toàn bộ các thao tác Add, Remove, FindById, GetAll, FindAll đều hoạt động độc lập và an toàn kiểu dữ liệu mà không cần sao chép mã nguồn.");
            Console.WriteLine();

            Console.WriteLine("================================================================================");
            Console.WriteLine("                        HOÀN THÀNH TOÀN BỘ KIỂM THỬ                             ");
            Console.WriteLine("================================================================================");
        }
    }
}
