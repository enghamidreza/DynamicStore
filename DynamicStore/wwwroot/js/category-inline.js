// فعال کردن حالت ویرایش روی سطر
function enableEdit(id) {
    const row = document.getElementById(`row-${id}`);
    row.querySelectorAll('.view-mode').forEach(el => el.classList.add('d-none'));
    row.querySelectorAll('.edit-mode').forEach(el => el.classList.remove('d-none'));
    document.getElementById(`title-input-${id}`).focus();
}

// انصراف از ویرایش
function cancelEdit(id) {
    const row = document.getElementById(`row-${id}`);
    const originalText = document.getElementById(`title-text-${id}`).innerText;
    document.getElementById(`title-input-${id}`).value = originalText;

    row.querySelectorAll('.edit-mode').forEach(el => el.classList.add('d-none'));
    row.querySelectorAll('.view-mode').forEach(el => el.classList.remove('d-none'));
}

// ذخیره تغییرات با AJAX
async function saveEdit(id) {
    const newTitle = document.getElementById(`title-input-${id}`).value;

    const formData = new FormData();
    formData.append('id', id);
    formData.append('title', newTitle);

    const response = await fetch('/Category/QuickEdit', {
        method: 'POST',
        body: formData
    });

    const result = await response.json();

    if (result.success) {
        document.getElementById(`title-text-${id}`).innerText = newTitle;
        cancelEdit(id);
    } else {
        alert(result.message);
    }
}

// حذف با AJAX
async function deleteCategory(id) {
    if (!confirm('آیا از حذف این دسته‌بندی اطمینان دارید؟')) return;

    const formData = new FormData();
    formData.append('id', id);

    const response = await fetch('/Category/Delete', {
        method: 'POST',
        body: formData
    });

    const result = await response.json();

    if (result.success) {
        const row = document.getElementById(`row-${id}`);
        row.remove();
    } else {
        alert(result.message || 'خطا در حذف دسته‌بندی.');
    }
}
