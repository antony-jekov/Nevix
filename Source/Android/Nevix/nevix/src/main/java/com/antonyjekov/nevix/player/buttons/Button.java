package com.antonyjekov.nevix.player.buttons;

import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Path;
import android.graphics.Rect;

/**
 * Created by Antony Jekov on 3/23/2014.
 */
public abstract class Button {

    protected final Rect button;
    private Path shapeStroke;
    private Paint paintStroke;

    protected int strokeWidth;
    protected int padding;

    public Rect buttonBounds() {
        return new Rect(this.button);
    }

    public Button(Rect button, int strokeWidth, int padding) {
        this.button = button;
        shapeStroke = new Path();

        paintStroke = new Paint(Paint.ANTI_ALIAS_FLAG);
        paintStroke.setStrokeWidth(strokeWidth);
        paintStroke.setStyle(Paint.Style.STROKE);
        paintStroke.setColor(Color.parseColor("#5ebdb8"));

        this.padding = padding;
        this.strokeWidth = strokeWidth;
    }

    public void renderSelf(Canvas canvas) {
        shapeStroke.reset();
        prepareShape(shapeStroke);

        canvas.drawPath(shapeStroke, paintStroke);

    }

    protected abstract void prepareShape(Path shape);

    public abstract String command();

    public boolean isPointInButton(float x, float y) {
        return x >= button.left && x <= button.right &&
                y >= button.top && y <= button.bottom;
    }
}
