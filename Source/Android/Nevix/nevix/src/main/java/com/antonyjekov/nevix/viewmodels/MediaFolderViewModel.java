package com.antonyjekov.nevix.viewmodels;

import java.util.ArrayList;

public class MediaFolderViewModel {
    private String name;
    private ArrayList<MediaFolderViewModel> folders;
    private ArrayList<MediaFileViewModel> files;

    public MediaFolderViewModel() {
        this.folders = new ArrayList<MediaFolderViewModel>();
        this.files = new ArrayList<MediaFileViewModel>();
    }

    public ArrayList<MediaFolderViewModel> getFolders() {
        return folders;
    }

    public void setFolders(ArrayList<MediaFolderViewModel> folders) {
        this.folders = folders;
    }

    public ArrayList<MediaFileViewModel> getFiles() {
        return files;
    }

    public void setFiles(ArrayList<MediaFileViewModel> files) {
        this.files = files;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
