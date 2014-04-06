package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

/**
 * Created by Antony Jekov on 3/23/2014.
 */
public abstract class SphericalButton extends Button {
    public SphericalButton(Rect button, int strokeWidth, int padding) {
        super(button, strokeWidth, padding);
    }

    @Override
    protected void prepareShape(Path shape) {
        shape.addCircle(button.centerX(), button.centerY(), button.height() >> 1, Path.Direction.CW);
    }
}
