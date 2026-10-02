$(document).ready(function () {
    const categorySelect = $('#categorySelect');
    const container = $('#attributesContainer');

    // دریافت مقادیر اولیه از اتریبیوت data
    let initialAttributeValues = [];
    try {
        const rawData = container.attr('data-initial-values');
        if (rawData) {
            initialAttributeValues = JSON.parse(rawData);
        }
    } catch (e) {
        console.error("خطا در پارس کردن ویژگی‌های اولیه:", e);
    }

    function loadAttributes(categoryId) {
        if (!categoryId) {
            container.html('<p class="text-muted mb-0">لطفاً ابتدا دسته‌بندی را انتخاب کنید.</p>');
            return;
        }

        container.html('<div class="text-center text-muted py-2"><span class="spinner-border spinner-border-sm me-2"></span>در حال بارگذاری...</div>');

        $.getJSON('/Products/GetCategoryAttributes?categoryId=' + categoryId, function (data) {
            container.empty();
            if (!data || data.length === 0) {
                container.html('<p class="text-muted mb-0">این دسته‌بندی ویژگی اختصاصی ندارد.</p>');
                return;
            }

            $.each(data, function (index, item) {
                // پیدا کردن مقدار ذخیره‌شده قبلی در صورت وجود
                let existingVal = "";
                const found = initialAttributeValues.find(v => (v.CategoryAttributeId || v.CategoryAttributeId) === item.id);
                if (found) {
                    existingVal = found.Value || found.value || "";
                }

                const inputHtml = `
                    <div class="mb-3">
                        <label class="form-label fw-semibold">${item.name}</label>
                        <input type="hidden" name="AttributeValues[${index}].CategoryAttributeId" value="${item.id}" />
                        <input type="text" name="AttributeValues[${index}].Value" value="${existingVal}" class="form-control" placeholder="مقدار را وارد کنید..." />
                    </div>`;
                container.append(inputHtml);
            });
        }).fail(function () {
            container.html('<div class="alert alert-danger py-2 mb-0">خطا در بارگذاری ویژگی‌ها!</div>');
        });
    }

    // لود اولیه در زمان باز شدن فرم
    if (categorySelect.val()) {
        loadAttributes(categorySelect.val());
    }

    // لود مجدد در صورت تغییر دسته‌بندی
    categorySelect.change(function () {
        // اگر دسته‌بندی تغییر کند، مقادیر اولیه دیگر مرتبط نیستند و پاک می‌شوند
        initialAttributeValues = [];
        loadAttributes($(this).val());
    });
});
