function confirmDelete(id, title) {
    if (confirm(`آیا از حذف محصول "${title}" مطمئن هستید؟`)) {
        const deleteInput = document.getElementById('deleteProductId');
        const deleteForm = document.getElementById('deleteForm');

        if (deleteInput && deleteForm) {
            deleteInput.value = id;
            deleteForm.submit();
        }
    }
}
