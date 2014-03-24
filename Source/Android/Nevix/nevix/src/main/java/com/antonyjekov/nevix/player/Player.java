package com.antonyjekov.nevix.player;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Rect;
import android.view.MotionEvent;
import android.view.View;

import com.antonyjekov.nevix.common.PusherManager;
import com.antonyjekov.nevix.player.buttons.Button;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Antony Jekov on 3/23/2014.
 */
public abstract class Player extends View {

    protected int width;
    protected int height;

    protected int halfWidth;
    protected int halfHeight;

    private final PusherManager pusher;
    protected int buttonMargin;

    protected List<Button> buttons;

    @Override
    protected void onDraw(Canvas canvas) {
        super.onDraw(canvas);
        for (Button button : buttons)
            button.renderSelf(canvas);
    }

    @Override
    public boolean onTouchEvent(MotionEvent event) {
        float x = event.getX();
        float y = event.getY();
        int action = event.getAction();
        if (action == MotionEvent.ACTION_DOWN) {
            onClick(x, y);
        }

        return super.onTouchEvent(event);
    }

    @Override
    protected void onSizeChanged(int w, int h, int oldw, int oldh) {
        super.onSizeChanged(w, h, oldw, oldh);
        width = w;
        height = h;
        halfHeight = h >> 1;
        halfWidth = w >> 1;

        buttonMargin = (int) (width * .03);

        arrangeButtons();
    }

    protected abstract void arrangeButtons();

    private void onClick(float x, float y) {
        for (Button btn : buttons) {
            if (btn.isPointInButton(x, y))
                pusher.pushCommand(btn.command());
        }
    }

    public Player(Context context, PusherManager pusher) {
        super(context);
        this.width = width;
        this.pusher = pusher;
        buttons = new ArrayList<Button>();
    }

    protected Rect rightTo(Rect relativeRect, int buttonSize) {
        int buttonHalfSize = buttonSize >> 1;
        int left = relativeRect.right + buttonMargin;
        return new Rect(left, relativeRect.centerY() - buttonHalfSize, left + (buttonHalfSize << 1), relativeRect.centerY() + buttonHalfSize);
    }

    protected Rect leftTo(Rect relativeRect, int buttonSize) {
        int halfSize = buttonSize >> 1;
        return new Rect(relativeRect.left - (buttonMargin + buttonSize), relativeRect.centerY() - halfSize, relativeRect.left - buttonMargin, relativeRect.centerY() + halfSize);
    }
}
