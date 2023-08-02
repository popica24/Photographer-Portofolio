var uploadButton = document.getElementById('uploadButton')
var deleteButton = document.getElementById('deleteButton')
var plusButton = document.getElementById('addButton')

var addedFiles = []
var deletedFiles = []

var previewContainer = document.getElementById('imagePreview')
var photosContainer = document.getElementById('photoGallery')



function handleFileSelect(input) {
    if (input.files && input.files.length > 0) {
        uploadButton.style.display = "block"

        for (var i = 0; i < input.files.length; i++) {
            var file = input.files[i]
            addedFiles.push(file)

            var reader = new FileReader()

            reader.onload = function (e) {
                var img = document.createElement('img')
                img.src = e.target.result
                img.classList.add('edit-container')
                previewContainer.appendChild(img)
            }

            reader.readAsDataURL(file)

        }

        var progressElement = document.getElementById('progress')
        progressElement.style.display = "block"
        progressElement.innerText = "Uploaded photo 0 /" + addedFiles.length
    }
}

function handleDeteleImage(input) {
    if (input) {
        deletedFiles.push(input)
        var photo = document.getElementById(input)
        previewContainer.appendChild(photo)
        deleteButton.style.display = "block"
        plusButton.style.display = "none"
    }
}

function deletePhoto(photoId, albumId) {
    var formData = new FormData()

    formData.append('albumId', albumId)
    formData.append('photoId', photoId)

    $.ajax({
        url: "/manager/delete/" + albumId + "/" + photoId + "/confirmed",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            console.log("Deleted photo with id " + id)
            document.getElementById(id).style.display = 'none'
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Failed to delete photo with id " + id)
        }
    })
}

function uploadFile(file, albumId) {
    var formData = new FormData();
    formData.append('Id', albumId);
    formData.append('Name', "");
    formData.append('Category', 0);
    formData.append('Images', file);

    return $.ajax({
        url: "/manager/edit/" + albumId,
        type: "POST",
        data: formData,
        processData: false,
        contentType: false
    });
}

deleteButton.addEventListener('click', function () {
    var albumId = this.getAttribute('data-albumid')

    for (var i = 0; i < deletedFiles.length; i++) {
        deletePhoto(deletedFiles[i], albumId)
    }
    previewContainer.innerHTML = ''
    deletedFiles = []
    plusButton.style.display = "block"
    deleteButton.style.display = "none"

})

uploadButton.addEventListener('click', function () {

    uploadButton.style.display = 'none'
    var progressElement = document.getElementById('progress')
    progressElement.innerHTML = 'Preparing upload...'
    var albumId = this.getAttribute('data-albumid')
    var addedFilesLength = addedFiles.length
    var currentPhotoIndex = 0

    function uploadNextPhoto() {
        if (currentPhotoIndex >= addedFilesLength) {
            addedFiles = []
            uploadButton.style.display = "none"
            progressElement.innerText = "All photos uploaded succesfully !";
            previewContainer.innerHTML = ''
            return
        }
        var currentPhoto = addedFiles[currentPhotoIndex]
        var photoIndex = currentPhotoIndex + 1
        progressElement.innerText = "Uploading photo " + photoIndex + '/' + addedFilesLength;
        uploadFile(currentPhoto, albumId)
            .done(function (response) {
                var photo = previewContainer.children[0]
                photosContainer.append(photo)
                currentPhotoIndex++
                uploadNextPhoto()
            })
            .fail(function (error) {
                progressElement.innerText = "Error uploading the photos with the following error message " + error.status
                return
            })
    }
    uploadNextPhoto()
})