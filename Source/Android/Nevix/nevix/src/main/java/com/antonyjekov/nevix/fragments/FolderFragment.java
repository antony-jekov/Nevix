package com.antonyjekov.nevix.fragments;

import android.os.Bundle;
import android.support.v4.app.ListFragment;
//import android.support.v7.appcompat.R;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Toast;

import com.antonyjekov.nevix.common.ContextManager;
import com.antonyjekov.nevix.common.contracts.OnFileSelected;
import com.antonyjekov.nevix.viewmodels.MediaFileViewModel;
import com.antonyjekov.nevix.viewmodels.MediaFolderViewModel;
import com.google.gson.Gson;

import java.util.ArrayList;

public class FolderFragment extends ListFragment {

    MediaFolderViewModel rootFolder;
    ContextManager db;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        db = new ContextManager(getActivity());
        String media = db.getMediaDatabase();

        rootFolder = loadMedia(media);
    }

    private MediaFolderViewModel loadMedia(String media) {
        ArrayList<MediaFolderViewModel> folders = new ArrayList<MediaFolderViewModel>();
        StringBuilder stringBuilder = new StringBuilder();
        int index = 0;
        boolean inBrackets = false;
        int curlyBracketsCount = 0;
        char[] mediaChars = media.toCharArray();
        int len = mediaChars.length;

        MediaFolderViewModel currentFolder;
        char currentChar;
        while (index < len) {
            currentChar = mediaChars[index++];

            if (currentChar == '"') {
                inBrackets = !inBrackets;
            } else if (!inBrackets && currentChar == '{') {
                curlyBracketsCount++;
            } else if (!inBrackets && currentChar == '}') {
                curlyBracketsCount--;
            }

            if (curlyBracketsCount > 0) {
                stringBuilder.append(currentChar);
            } else if (curlyBracketsCount == 0 && stringBuilder.length() > 0) {
                stringBuilder.append('}');
                currentFolder = new Gson().fromJson(stringBuilder.toString(), MediaFolderViewModel.class);
                folders.add(currentFolder);
                stringBuilder.setLength(0);
            }
        }

        MediaFolderViewModel root;
        if (folders.size() == 1) {
            root = folders.get(0);
        } else {
            root = new MediaFolderViewModel();
            root.setName("Root");
            root.setFolders(folders);
        }

        return root;
    }

    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        openFolder(rootFolder);
    }

    private void openFolder(MediaFolderViewModel rootFolder) {
        ArrayList<String> contents = new ArrayList<String>();
        for (MediaFolderViewModel folder : rootFolder.getFolders()) {
            contents.add(folder.getName());
        }

        for (MediaFileViewModel file : rootFolder.getFiles()) {
            contents.add(file.getName());
        }

        ArrayAdapter adapter = new ArrayAdapter(getActivity(), android.R.layout.simple_list_item_1, contents);
        setListAdapter(adapter);
    }

    @Override
    public void onListItemClick(ListView l, View v, int position, long id) {
        int index = position;
        int foldersCount = rootFolder.getFolders().size();
        int filesCount = rootFolder.getFiles().size();
        if (index >= foldersCount) {
            index -= foldersCount;
            OnFileSelected activity = (OnFileSelected) getActivity();
            activity.onFileSelected(rootFolder.getFiles().get(index).getId());
        } else {
            MediaFolderViewModel selectedFolder = rootFolder.getFolders().get(index);
            openFolder(selectedFolder);
            rootFolder = selectedFolder;
        }
    }
}
