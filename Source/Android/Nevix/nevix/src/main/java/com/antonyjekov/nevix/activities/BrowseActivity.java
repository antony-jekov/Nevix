package com.antonyjekov.nevix.activities;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ScrollView;
import android.widget.Toast;

import com.antonyjekov.nevix.R;
import com.antonyjekov.nevix.common.ContextManager;
import com.antonyjekov.nevix.common.FolderAdapter;
import com.antonyjekov.nevix.common.HttpAsyncRequest;
import com.antonyjekov.nevix.common.MediaItem;
import com.antonyjekov.nevix.common.PersistentManager;
import com.antonyjekov.nevix.common.contracts.OnFileSelected;
import com.antonyjekov.nevix.viewmodels.MediaFileViewModel;
import com.antonyjekov.nevix.viewmodels.MediaFolderViewModel;
import com.google.gson.Gson;

import java.util.ArrayList;
import java.util.Stack;

public class BrowseActivity extends BaseActivity implements OnFileSelected {

    Button backBtn;
    Button forwardBtn;

    Stack<MediaFolderViewModel> backQueue;
    Stack<MediaFolderViewModel> forwardQueue;

    private LinearLayout list;
    private String lastServerUpdate;
    private ContextManager db;
    private PersistentManager persistentManager;
    private ScrollView mediaList;
    private MediaFolderViewModel rootFolder;

    public static final String BROWSED_FILE = "com.antonyjekov.nevix.browse.browsedFile";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        backQueue = new Stack<MediaFolderViewModel>();
        forwardQueue = new Stack<MediaFolderViewModel>();
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_browse);
        db = new ContextManager(this);
        String media = db.getMediaDatabase();
        getSupportActionBar().hide();
        rootFolder = loadMedia(media);

        this.mediaList = (ScrollView) findViewById(R.id.media_list);
        list = (LinearLayout) findViewById(R.id.jekozo);

        String sessionKey = db.getSessionKey();
        persistentManager = new PersistentManager(sessionKey);

        openRootFolder();

        final String lastLocalUpdate = db.getLastDatabaseUpdate();
        persistentManager.getLastMediaUpdateTime(new HttpAsyncRequest.OnResultCallBack() {
            @Override
            public void onResult(String result) {
                lastServerUpdate = result;
                if (result == null || !result.equals(lastLocalUpdate)) {
                    beginMediaSync();
                }
            }
        });

        backBtn = (Button) findViewById(R.id.back_btn);
        backBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                forwardQueue.push(rootFolder);
                rootFolder = backQueue.pop();
                openRootFolder();
                updateButtons();
            }
        });


        forwardBtn = (Button) findViewById(R.id.forward_btn);
        forwardBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                backQueue.push(rootFolder);
                rootFolder = forwardQueue.pop();
                openRootFolder();
                updateButtons();
            }
        });

        View syncBtn = findViewById(R.id.sync_btn);
        syncBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                beginMediaSync();
            }
        });

        backBtn.setEnabled(false);
        forwardBtn.setEnabled(false);
    }

    private void updateButtons() {
        backBtn.setEnabled(backQueue.size() > 0);
        forwardBtn.setEnabled(forwardQueue.size() > 0);
    }

    private void beginMediaSync() {

        printMessage(getResources().getString(R.string.beginning_media_sync));

        persistentManager.getMedia(new HttpAsyncRequest.OnResultCallBack() {
            @Override
            public void onResult(String result) {
                db.storeMediaDatabase(result);
                db.setLastDatabaseUpdate(lastServerUpdate);
                printMessage("Sync completed");
                rootFolder = loadMedia(result);
                openRootFolder();
                backQueue.clear();
                forwardQueue.clear();
                backBtn.setEnabled(false);
                forwardBtn.setEnabled(false);
            }
        });
    }

    private void printMessage(String message) {
        Toast.makeText(this, message, Toast.LENGTH_LONG).show();
    }

    @Override
    public void onFileSelected(int fileId) {
        Intent data = new Intent();
        data.putExtra(BROWSED_FILE, fileId);
        setResult(RESULT_OK, data);
        finish();
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

    private void openRootFolder() {
        list.removeAllViews();
        final ArrayList<MediaItem> contents = new ArrayList<MediaItem>();
        int folderIcon = R.drawable.media_folder2;
        int movieIcon = R.drawable.media_icon;
        int count = 0;

        for (MediaFolderViewModel folder : rootFolder.getFolders()) {
            contents.add(new MediaItem(folder.getName(), folderIcon, count++));
        }

        for (MediaFileViewModel file : rootFolder.getFiles()) {
            contents.add(new MediaItem(file.getName(), movieIcon, count++));
        }

        ArrayAdapter adapter = new FolderAdapter(this, R.layout.media_item, contents.toArray());

        for (int i = 0; i < count; i++) {
            View mediaItem = adapter.getView(i, null, this.mediaList);
            mediaItem.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    for (int view = 0; view < list.getChildCount(); view++)
                        if (list.getChildAt(view) == v)
                            onListItemClick(view);
                }
            });
            list.addView(mediaItem);
        }
    }

    public void onListItemClick(int position) {
        forwardQueue.clear();
        int index = position;
        int foldersCount = rootFolder.getFolders().size();

        if (index >= foldersCount) {
            index -= foldersCount;
            this.onFileSelected(rootFolder.getFiles().get(index).getId());
        } else {
            MediaFolderViewModel selectedFolder = rootFolder.getFolders().get(index);
            try {
                backQueue.push((MediaFolderViewModel) rootFolder.clone());
            } catch (CloneNotSupportedException e) {
                e.printStackTrace();
            }
            rootFolder = selectedFolder;
            openRootFolder();
        }
        updateButtons();
    }
}