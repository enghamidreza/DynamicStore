document.addEventListener('DOMContentLoaded', function () {
    const categorySelect = document.getElementById('categorySelect');
    const container = document.getElementById('dynamicAttributesContainer');

    if (!categorySelect || !container) return;

    categorySelect.addEventListener('change', async function () {
        const categoryId = this.value;
        container.innerHTML = '';

        if (!categoryId) {
            container.innerHTML = '<div class="text-muted">لطفاً ابتدا دسته‌بندی را انتخاب کنید.</div>';
            return;
        }

        try {
            const response = await fetch(`/Products/GetCategoryAttributes?categoryId=${categoryId}`);
            if (!response.ok) throw new Error('خطا در ارتباط با سرور');

            const attributes = await response.json();

            if (attributes.length === 0) {
                container.innerHTML = '<div class="text-info">این دسته ویژگی خاصی ندارد.</div>';
                return;
            }

            attributes.forEach((attr, index) => {
                const col = document.createElement('div');
                col.className = 'col-md-6';
                col.innerHTML = `
                    <label class="form-label">${attr.title}</label>
                    <input type="hidden" name="AttributeValues[${index}].CategoryAttributeId" value="${attr.id}" />
                    <input type="text" name="AttributeValues[${index}].Value" class="form-control" />
                `;
                container.appendChild(col);
            });
        } catch (error) {
            console.error('خطا در دریافت ویژگی‌ها:', error);
            container.innerHTML = '<div class="text-danger">خطا در بارگذاری ویژگی‌ها.</div>';
        }
    });
});
